<?php
          // Function: Returns given ID's data about what items it has unlocked.
          // Format is "[ITEMNAME]=[UNLOCKSTATUS]", where itemname = string
          // and unlockstatus = 0 or 1, where 0 = locked & 1 = unlocked.

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
            $table      = "Unlock_ID_$tableID";
          }
          else
          {
           echo "FAILURE: No POST";
           exit();
          }
          // script start
          $select = "SELECT * FROM $table";

          $select_query = mysqli_query($connection, $select);
          if (!$select_query)
          {
            die("FAILURE: Requested table not found". '<br />' . mysqli_error($connection));
          }
          // table exists -> proceed to echo the results
          else{
            $fetch = mysqli_query($connection,$select);
            while ($forValue = mysqli_fetch_array($fetch))
            {
              echo $forValue['itemName'] . "=" . $forValue['unlocked'] . ":". '<br />';
            }
            // should return each row in the table as their own row, but C# doesn't seem to care either way
          }
   mysqli_close($connection);
?>
