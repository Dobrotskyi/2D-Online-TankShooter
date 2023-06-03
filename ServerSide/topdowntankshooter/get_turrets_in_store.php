<?php
$con = mysqli_connect('localhost', 'root', 'root', 'topdowntankshooter');

if (mysqli_connect_errno()) {
    echo "1 Connection failed";
    exit();
}
$nickname = $_POST["nickname"];
$purchased= $_POST["purchased"];
$switch = "";

if ($purchased == "True")
    $switch = "IN";
else
    $switch = "NOT IN";

$queryText = "Select tank_turrets.name, tank_turrets.rotation_speed , tank_turrets.spread_x, tank_turrets.spread_y,
                tank_turrets.fire_rate, tank_turrets.shot_force, tank_turrets.durability_mult,
                tank_turrets.damage_mult,ANY_VALUE(tank_turrets_sprites.sprite) as 'turret_sprite',
                projectiles.name as 'projectile_name',
                projectiles.damage, projectiles.time_of_life, projectiles.ammo_cost, ANY_VALUE(projectiles_sprites.sprite) as 'projectile_sprite'
                from tank_turrets
                INNER join tank_turrets_sprites on tank_turrets.id = tank_turrets_sprites.turret_id
                INNER JOIN projectiles on tank_turrets.projectile_id = projectiles.id
                INNER join projectiles_sprites on projectiles.id = projectiles_sprites.projectile_id
                where tank_turrets.id " . $switch . " 
                                (SELECT tank_turrets.id                              
                                from users_turrets 
                                INNER join tank_turrets on users_turrets.tank_turret_id = tank_turrets.id
                                INNER join users on users_turrets.user_id = users.id
                                WHERE users.nickname = '" . $nickname . "')
                                GROUP by tank_turrets.id;";


$result = mysqli_query($con, $queryText);
$rows_amt = mysqli_num_rows($result);

if ($rows_amt == 0)
    die("0" . "All items were bought");
else {
    echo ("0");
    for ($i = 0; $i < mysqli_num_rows($result); $i++) {
        $rowInfo = mysqli_fetch_assoc($result);
        $rowInfo["turret_sprite"] = base64_encode($rowInfo["turret_sprite"]);
        $rowInfo["projectile_sprite"] = base64_encode($rowInfo["projectile_sprite"]);
        echo (implode(",", $rowInfo) . ",\t");
        if ($i != $rows_amt - 1)
            echo (",");
    }
    exit();
}
?>