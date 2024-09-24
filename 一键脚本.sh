#!/bin/bash

# 一键推送的函数
push_changes() {
    # 获取所有远程仓库列表
    remotes=$(git remote)
    
    # 添加所有更改
    git add .
    
    # 提交更改
    echo "请输入提交信息: "
    read commit_message
    git commit -m "$commit_message"
    
    # 推送更改到所有远程仓库
    for remote in $remotes; do
        branch=$(git rev-parse --abbrev-ref HEAD)
        git push $remote $branch
    done

    echo "更改已推送到所有远程仓库。"
}

# 一键拉取的函数
pull_changes() {
    # 获取所有远程仓库列表
    remotes=$(git remote)
    
    echo "请选择一个远程仓库来拉取更新:"
    select remote in $remotes; do
        branch=$(git rev-parse --abbrev-ref HEAD)
        git pull $remote $branch
        break
    done

    echo "已从所选远程仓库拉取更新。"
}

# 主菜单
while true; do
    echo "请选择一个选项:"
    echo "1) 一键推送"
    echo "2) 一键拉取"
    echo "3) 退出"

    read choice

    case $choice in
        1)
            push_changes
            ;;
        2)
            pull_changes
            ;;
        3)
            break
            ;;
        *)
            echo "无效选项。请重试。"
            ;;
    esac
done