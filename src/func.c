#include <windows.h>
#include <stdio.h>
#include <stdlib.h>

#define BUFFER_SIZE 1024
#define RESPONSE_BUFFER_SIZE 1024 * 10  // 10 KB

const char *executeCommand(const char *cmd)
{
    FILE *fp;
    static char total_response[RESPONSE_BUFFER_SIZE];
    char container[BUFFER_SIZE];

    memset(total_response, 0, sizeof(total_response));
    memset(container, 0, sizeof(container));

    fp = _popen(cmd, "r");
    
    while(fgets(container, BUFFER_SIZE, fp) != NULL) 
    {
        strcat(total_response, container);
    }

    _pclose(fp);

    return total_response;
}