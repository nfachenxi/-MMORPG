@echo off
setlocal enabledelayedexpansion

rem 更改命令行编码为 UTF-8
chcp 65001 >nul

rem 提示用户输入需要恢复的文件名
echo Please enter the name of the file you want to restore:
set /p filename=

rem 检查是否已经在仓库内
if not exist .git (
    echo This directory is not a Git repository. Please run this script in a Git repository.
    pause
    exit /b 1
)

rem 查找包含文件的最近提交
for /f "tokens=* delims=|" %%i in ('git log --format="%H|%s" -- %filename^| findstr /R /C:"delete mode 100644 %filename%"') do (
    set commit_with_file=%%i
)

if defined commit_with_file (
    rem 提取提交哈希
    set commit_hash=!commit_with_file:~0,40!

    rem 回退到包含文件的提交
    echo Restoring to the commit containing %filename% (%commit_hash%)...
    git reset --hard %commit_hash%
    if %errorlevel% NEQ 0 (
        echo Failed to reset. Please check for errors!
        pause
        exit /b 1
    )
    echo File %filename% has been successfully restored!
) else (
    echo No commit found that contains %filename%. Please check the filename.
    pause
    exit /b 1
)

pause
endlocal
exit /b 0