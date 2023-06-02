<?php
function nicknameIsUsed(mysqli $con, string $name)
{
    $nameCheckQuery = "Select nickname
                   from users
                   Where nickname = '" . $name . "';";
    $namecheck = mysqli_query($con, $nameCheckQuery) or die("2 Namecheck has failed");

    if (mysqli_num_rows($namecheck) > 0)
        return true;
    else
        return false;
}

function tryLogin(mysqli $con, string $name, string $password)
{
    if (!nicknameIsUsed($con, $name))
        return "No such user";

    $getLoginInfo = "Select nickname, salt, hash, money
                   from users
                   Where nickname = '" . $name . "';";

    $result = mysqli_query($con, $getLoginInfo) or die("Login try was failed");

    $result = mysqli_fetch_assoc($result);
    $salt = $result["salt"];
    $hash = $result["hash"];

    $tryloginhash = crypt($password, $salt);

    if ($hash != $tryloginhash)
        return "Incorrect password";
    else
        return $result;
}
?>