Compose
======

After switching from a EU ISO keyboard layout to the US ANSI, I lost the ability to write some of the foreign characters used in my language, and some other symbols featured in the AltGr layer of my old layout.

So, instead of using the character map like a scrub, I made this tool.

Features & How to use
======

It resurrects the [compose key](https://en.wikipedia.org/wiki/Compose_key) functionality of keyboards from ye olde days into something a bit more customizable like a software tool.

The usage is simple, you hit a designated hotkey that acts as the compose key, followed by a two-letter combination and voilà [1], you've got yourself some snazzy looking unicode symbols!

[1] The à "voilà" was written using this tool, by first pressing the compose key (which I've set to the right ctrl key), followed by "`" and finally a lowercase a.

If you want to test and use it, download the [latest release](https://github.com/p3lim/Compose/releases) and run the executable.

Right-click the tray icon to access settings where you'll be able to bind your key.

======

A list of all combinations currently supported, including description and result, can be found here:

[Compose/Symbols.md](https://github.com/p3lim/Compose/blob/master/symbols.md)

Here are some of my favorites:

| Unicode | Combination | Symbol | Details |
|:-:|:-:|:-:|:--|
| 00a3 | L- | £ | Pound sign |
| 20ac | c= | € | Euro sign |
| 00a9 | oc | © | Copyright sign |
| 00ae | or | ® | Registered sign |
| 2122 | tm | ™ | Trade mark |
| 00b0 | oo | ° | Degree sign |
| 00bd | 12 | ½ | Fraction (half) |
| 00c5 | oA | Å | Latin capital letter A with ring above |
| 00c6 | AE | Æ | Latin capital letter AE |
| 00d8 | /O | Ø | Latin capital letter O with stroke |


Disclaimer
======

This software has only been tested on Windows 7 and later, I can not guarantee it will work on any earlier systems.

Please report [any bugs](https://github.com/p3lim/Compose/issues?q=is%3Aopen+is%3Aissue+label%3Abug) you might find, and feel free to suggest [a feature](https://github.com/p3lim/Compose/issues?q=is%3Aopen+is%3Aissue+label%3Aenhancement).


Contributing
======

This is the first software I've ever developed on the Windows platform (and using C#), so I have no idea if the code is up to par with any convention.

Feel free to contribute by [forking](https://github.com/p3lim/Compose/fork) and sending me [pull requests](https://github.com/p3lim/Compose/pulls?q=is%3Aopen+is%3Apr).


Legal
======

See the [LICENSE](https://github.com/p3lim/Amp/blob/master/LICENSE) file.

The [compose icon](https://github.com/p3lim/Compose/tree/master/Icon) is licensed under the [MIT License](https://github.com/p3lim/Compose/blob/master/Icon/LICENSE) by [@jxnblk](https://github.com/jxnblk).
