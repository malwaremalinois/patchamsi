Basic C# to bypass AMSI 
//First je change to jne in amsiinitialize so that error is thrown and catch goes to 
// B8 57000780              | mov eax,80070057              
// and bypasses asmi
//based on HAMSICONTEXT somewhat
really helpful article -->https://code-white.com/blog/2019-07-heap-based-amsi-bypass-in-vba/

TODO: Additional context to corrupt HAMSICONTEXT in C/C++