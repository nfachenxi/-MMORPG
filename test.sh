#!/bin/bash
# 设置默认推送分支为 master
branch=master

# 设置编码为UTF-8
export LANG=en_US.UTF-8

# 设置远程仓库名称
repos=("gitee" "github" "mygit")

# 设置提交信息
read -p "请输入提交信息：" commit_message

# 添加所有更改
echo "正在添加所有更改..."
git add .

# 提交更改
echo "正在提交更改..."
git commit -m "$commit_message"
if [ $? -ne 0 ]; then
    echo "提交失败，请检查错误！"
    read -p "按任意键继续..." -n 1 -s
    echo
    exit 1
else
    echo "提交成功！"
fi

# 遍历所有远程仓库并推送
for repo in "${repos[@]}"; do
    echo "正在向远程仓库 $repo 推送分支 $branch..."

    # 尝试推送代码
    git push "$repo" "$branch"

    if [ $? -ne 0 ]; then
        echo "向远程仓库 $repo 推送失败，请检查错误！"
        read -p "按任意键继续..." -n 1 -s
        echo
        exit 1
    else
        echo "向远程仓库 $repo 推送成功！"
    fi
done

read -p "按任意键继续..." -n 1 -s
echo
exit 0