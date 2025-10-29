#include "pch.h"
#include "LogHelper.h"

#include <comdef.h>

#include "NetExecutor.h"

// std::unique_ptr<FrameworkExecutor> GetExecutor(const std::wstring& framework)
std::unique_ptr<FrameworkExecutor> GetExecutor()
{
	// LogHelper::WriteLine(L"Trying to get executor for framework '%s'...", framework.c_str());

	//if (icase_cmp(framework, L"net6.0-windows"))
	//{
		return std::make_unique<NetExecutor>();
	//}

	//if (icase_cmp(framework, L"net462"))
	//{
	//	MessageBoxW(NULL, L"Code for .net framework is removed. Will not work", 
	//		L"Not for NetFramework", MB_OK | MB_ICONINFORMATION);
	//	return 0;
	//}

	//LogHelper::WriteLine(L"Framework '%s' is not supported.", framework.c_str());

	//return nullptr;
}

extern "C" __declspec(dllexport) int STDMETHODVCALLTYPE ExecuteInDefaultAppDomain(const LPCWSTR input)
{
	try
	{
		LogHelper::WriteLine(input);
		const auto parts = split(input, L"<|>");

		if (parts.size() < 5)
		{
			// LogHelper::WriteLine(L"Not enough parameters.");
			MessageBoxW(NULL, L"Not enough parameters",
				L"Erorro ", MB_OK | MB_ICONINFORMATION);
			return E_INVALIDARG;
		}

		const auto& assemblyPath = parts.at(0);
		const auto& className = parts.at(1);
		const auto& method = parts.at(2);
		const auto& parameterJson = parts.at(3);
		const auto& logFile = parts.at(4);

		LogHelper::SetLogFile(logFile);
		LogHelper::WriteLine(input);

		// const auto executor = GetExecutor(framework);
		const auto executor = GetExecutor();

		if (!executor)
		{
			LogHelper::WriteLine(L"No executor found.");
			return E_NOTIMPL;
		}

		DWORD* retVal = nullptr;
		const auto hr = executor->Execute(assemblyPath.c_str(), className.c_str(), method.c_str(), parameterJson.c_str(), retVal);

		if (FAILED(hr))
		{
			MessageBoxW(NULL, L"FAiled ...",
				L"Failed Failed", MB_OK | MB_ICONINFORMATION);

			const _com_error err(hr);

			LogHelper::WriteLine(L"Error while calling '%s' on '%s' from '%s' with '%s'", method.c_str(), className.c_str(), assemblyPath.c_str(), parameterJson.c_str());
			LogHelper::WriteLine(L"HResult: %i", hr);
			LogHelper::WriteLine(L"Message: %s", err.ErrorMessage());
			LogHelper::WriteLine(L"Description: %s", std::wstring(err.Description(), SysStringLen(err.Description())).c_str());
		}
		else
		{
			LogHelper::WriteLine(L"Done calling '%s' on '%s' from '%s' with '%s'", method.c_str(), className.c_str(), assemblyPath.c_str(), parameterJson.c_str());
		}


		return hr;
	}
	catch (std::exception& exception)
	{
		LogHelper::WriteLine(L"ExecuteInDefaultAppDomain failed with exception.");
		LogHelper::WriteLine(L"Exception:");
		LogHelper::WriteLine(to_wstring(exception.what()));
	}
	catch (...)
	{
		LogHelper::WriteLine(L"ExecuteInDefaultAppDomain failed with unknown exception.");
	}

	return E_FAIL;
}