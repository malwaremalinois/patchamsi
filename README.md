Basic C# to bypass AMSI  
\n First je change to jne in amsiscanbuffer so that error is thrown and catch goes to  
 B8 57000780              | mov eax,80070057  
 and bypasses asmi  
based on HAMSICONTEXT somewhat  
really helpful article -->https://code-white.com/blog/2019-07-heap-based-amsi-bypass-in-vba/  
(tested windows 11)  
TODO:  
-- Additional context to corrupt HAMSICONTEXT in C/C++  
-- Need to address that offset needs to be dynamically found not static at +54 -> can search for instructions test edi,edi  