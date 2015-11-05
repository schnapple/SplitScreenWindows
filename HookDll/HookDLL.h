#pragma once
#ifndef __HOOK__DLL__
#define __HOOK__DLL__

#ifdef __cplusplus
extern "C" {
#endif
#ifdef DLLDEF
#define DLLSPEC __declspec(dllexport)
#else
#define DLLSPEC __declspec(dllimport)
#endif //DLLDEF
	DLLSPEC bool installHook();
#undef DLLSPEC
#ifdef __cplusplus
}
#endif //__cplusplus
#endif //__HOOK__DLL__