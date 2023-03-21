# Rozbie

Version: 0.1.0

This is a simple and very standard malware code that provide an access to the infected host.
The Rozbie works only on Windows OS.

## Project

The presented soluction consists of two project:

1. Rozbie malware written in Rust. It is a program that must be run in the target system to give an intruder an access to the system's command line console.
2. Rozbie Farm is a server in which you can execute commands to run on infected with Rozbie computers.

![Rozbie Farm](https://github.com/rozensoftware/rozbie/blob/master/RozbieFarm.jpg)

## Installation

It is up to you how you install Rozbie on the target computer. In the following updates you should find information how to do it.
On successful run the Rozbie will add itself to the Windows current user autorun. It should start up after the user logged into system.
When it is there it will try to connect to The Rozbie Farm application once per 3 seconds.
You should be able to type DOS commands in the Rozbie Farm appliaction to be executed on the infected computer.

You must change the *IP* address and eventually the port to yours in the main.rs:

```rust
const SERVER_IP: &'static str = "192.168.0.22";
const SERVER_PORT: &'static str = "1973";
```

The same must be changed on the C# side in the Server.cs file:

```csharp
private static readonly int SERVER_PORT = 1973;
private static readonly string SERVER_IP = "192.168.0.22";
```

## Special commands

The Rozbie should execute almost every DOS command on the infected host.
There is a special command that can be executed only by the Rozbie:

1. 'q' - Shutdowns Rozbie.

**Note:** After user logged into the system current directory of the Rozbie should be Windows\System32 which can include thousands of files.
It's better to type first: 'cd ..' to change directory to one level up instead of 'dir'. There is a static buffer which can hold up to 1MB of data only.
You can chage it to a bigger value of course. It is something to address in future commits but as long as it is just a small vicious program, some optimizations are missing now.

## Disclaimer

The author of this code is not responsible for the incorrect operation of the presented code and/or for its incorrect use. The code presented in this project is intended to serve only to learn programming.

## License

This project is licensed under MIT

MIT license (LICENSE-MIT or <http://opensource.org/licenses/MIT>)

## Contributing / Feedback

I am always glad to learn from anyone.
If you want to contribute, you are more than welcome to be a part of the project! Try to share you thoughts first! Feel free to open a new issue if you want to discuss new ideas.

Any kind of feedback is welcome!
