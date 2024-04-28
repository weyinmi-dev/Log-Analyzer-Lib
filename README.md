# Log Analyzer Library

This repository contains a C# implementation of a Log Analyzer Library that provides functionalities for analyzing logs in multiple directories. The project includes an API controller that executes each function of the ILogAnalyzerUserStories service.

**Features**

The Log Analyzer Library offers the following features:

- Search Logs: Search for log files in specified directories.

- Count Unique Errors: Count the number of unique errors in log files.

- Count Duplicated Errors: Count the number of duplicated errors in log files.

- Delete Archive from a Period: Delete archived log files from a specified time period.

- Archive Logs from a Period: Archive log files from a specified time period and store them in a zip file.

- Upload Logs to a Remote Server: Upload log files to a remote server using an API.

- Delete Logs from a Period: Delete log files from a specified time period.

- Count Total Available Logs in a Period: Count the total number of log files in a specified time period.

- Search Logs per Size: Search log files within a specified size range.

- Search Logs per Directory: Search log files in a specified directory.


**Requirements**

- .NET 6 or higher.

- Internet connection for remote server functionality (if applicable).


**Getting Started**

Clone the repository:

- Copy code git clone [https://github.com/weyinmi-dev/Log-Analyzer-Lib]
- cd(Create directory) log-analyzer-library

**Set up the project:**

- Open the project in your preferred IDE.
- Install this required package(s) using your IDE's NuGet package manager: ``Onax.Tools.Utils``
- Ensure your .NET environment is set up and configured.
- Run the application:
- Build and run the project.

**API Endpoints:**

The API controller defines endpoints for each function of the ILogAnalyzerUserStories service. You can refer to the source code for the list of available endpoints and their respective HTTP methods.

**Usage**

You can use the API controller to interact with the Log Analyzer Library. Refer to the source code for endpoint details and examples. To interact with the API, you can use HTTP client tools such as Postman or curl.

**Contributions**

Contributions to the project are welcome. If you find a bug or have a feature request, please open an issue. Feel free to submit pull requests with improvements.

**License**

This project is licensed under the MIT License. Please refer to the license file for more information.

**Acknowledgments**

Special thanks to the open-source community for making this project possible.
Thanks to everyone who has contributed to this project.

**Contact**

For any inquiries or feedback, please reach out via GitHub issues.
