<?php
        // FUNCTION: Unlocks a given stage (within specified world) for specified ID.
        // Notice: Target world-stage is the stage you want to UNLOCK, not the stage that unlocks it.

        $server   = "localhost";
        $username = "root";
        $password = "";

        $database = "PlayerData";

        $connection = mysqli_connect($server, $username, $password, $database) or die("Cant connect into database");
        mysqli_select_db( $connection, $database) or  die("Cant connect into database");
        if($connection === false)
        {
          die("ERROR: Could not connect.". '<br />' . mysqli_connect_error());
        }

        if ($_POST)
        {
          $tableID    = mysqli_real_escape_string($connection, $_POST['ID']);
          $table      = "ID_$tableID";
          $world      = mysqli_real_escape_string($connection, $_POST['world']);
          $stage      = mysqli_real_escape_string($connection, $_POST['stage']);
        }
        else
        {
         echo "FAILURE: No POST";
         exit();
        }

        // script start
        // basically simpler and modifier "StageDataRewrite"
        // 1. Checks for table and row existence
        // 2. Checks if the stage has already been unlocked
        // 3. Unlocks the stage with UPDATE
        $select = "SELECT * FROM $table
                   WHERE world=$world
                   AND stage=$stage";
        if (!mysqli_query($connection, $select))
        {
          die ("FAILURE: Requested stage (row) not found.". '<br />' . mysqli_error($connection));
        }
        elseif (mysqli_fetch_array(mysqli_query($connection, $select))['unlocked'] == 1)
        {
          die ("FAILURE: Stage $world-$stage already unlocked.");
        }
        else {
          $update = "UPDATE $table
                    SET unlocked=1
                    WHERE world=$world
                    AND stage=$stage";
          if (mysqli_query($connection, $update))
          {
            echo "SUCCESS: Stage $world-$stage unlocked (player $tableID)";
          }
          else {
            die("ERROR: Stage $world-$stage unlock failed.". '<br />' . mysqli_error($connection));
          }
        }
    mysqli_close($connection);
?>
