# BotSuite.NET

BotSuite.NET is a library offering a framework which can be used to develop "bots" (as in "every programm that performs a task automatically") without having to think about "low level" work.  
It combines power and usability to provide an easy to use and solid base for every bot you want to create.

### How to use the BotSuite.NET

Your best bet is to go to the downloads and get the latest version there.  
The download should contain:

* The library (**BotSuite.dll**)
* A XML file (**BotSuite.xml**), which provides the code documentation (as in "IntelliSense")
* A file containing license information (**license**)

After downloading the BotSuite, just put the DLL and the XML whereever you want (together) and then add a reference to the DLL in your .NET project.  
The entry point of the BotSuite is the "BotSuite" namespace, containing classes and sub namespaces.

Following namespaces might be important:

* **BotSuite.Imaging**: Classes needed for image processing.
* **BotSuite.Input**: Classes for controlling mouse and keyboard. Also contains a HotKey class to react to user input.
* **BotSuite.Net**: Classes for work with web pages. The class Browser encapsulates the locally installed internet explorer while the HttpClient is for low level HTTP(S) requests.

### Contact

You can contact me in different ways:

* If there is a problem with the BotSuite, please open an issue
* If you want to contribute to the BotSuite, please make a fork and create pull requests with your changes
* If you want to get (very frequent) updates on my projects (including the BotSuite), you can follow me on Twitter: https://twitter.com/KarillEndusa
* There are several (german) communities where you can find me: http://forum.mds-tv.de and http://codebot.de

### Tutorials / Documentation

A rudimentary documentation exists in the form of code comments for classes and methods.  
I have planned to create a more detailed documentation and some tutorials covering the very basics.  
If you want to help with either one feel free to contact me.

### License

This project is licensed under the BSD 3-Clause License.

Copyright (c) 2013-2014, wieschoo & Binary Overdrive
Copyright (c) 2013-2015, Binary Overdrive  
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
