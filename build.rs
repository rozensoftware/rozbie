fn main() 
{
    println!("cargo:rerun-if-changed=src/hideinjector.c");
    println!("cargo:rerun-if-changed=src/func.c");
    
    #[cfg(target_os = "windows")]
    cc::Build::new()
        .file("src/func.c")
        .compile("cfuncs");
}
