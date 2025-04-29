# Lấy đường dẫn thư mục hiện tại
$repoPath = Get-Location

# Nội dung file .gitconfig-local
@"
[user]
    name = Nguyễn Việt Hưng
    email = hungcutedethuongg@gmail.com
"@ | Out-File -Encoding utf8 "$repoPath\.gitconfig-local"

# Đường dẫn file gitconfig toàn cục
$globalGitConfig = "$env:USERPROFILE\.gitconfig"

# Kiểm tra và thêm includeIf nếu chưa có
$searchString = "gitdir:$repoPath\"

if (-not (Select-String -Path $globalGitConfig -Pattern [regex]::Escape($searchString) -Quiet)) {
    Add-Content -Path $globalGitConfig -Value ""
    Add-Content -Path $globalGitConfig -Value "[includeIf ""gitdir:$repoPath\""]"
    Add-Content -Path $globalGitConfig -Value "    path = $repoPath\.gitconfig-local"
}

Write-Output "Đã tạo cấu hình cho repo: $repoPath"
