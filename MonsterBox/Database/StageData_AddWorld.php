<?php
          // Adds next unused world and given number of stages to all tables
          // (Accounts for all tables and further table creation)
          // obviously all stored data will be empty
          // eg. if the game currently stores 3 worlds, it adds world #4.

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
            $stageNumber      = mysqli_real_escape_string($connection, $_POST['stageCount']);
          }
          else
          {
           echo "FAILURE: No POST";
           exit();
          }

          // script start

          if ($stageNumber <= 0)
          {
            die ("ERROR: Incorrect stageCount given. (stageCount must be positive)");
          }


          // mastertable search
          $select = "SELECT * FROM mastertable
                    WHERE identification=1";
          $select_query = mysqli_query($connection, $select);
          if (!$select_query)
          {
            die("ERROR: mastertable doesn't exist");
          }
          $userCount = mysqli_fetch_array(mysqli_query($connection,$select))['usercount'];
          $stageArray= mysqli_fetch_array(mysqli_query($connection,$select))['stageArray'];
          $worldCount= mysqli_fetch_array(mysqli_query($connection,$select))['worldCount'];

          // next world in line....
          $world = $worldCount+1;


          // inserting the item row to each table
          for ($i1 = 1; $i1 < ($userCount+1); $i1++)
          {
            for ($i2 = 1; $i2 < ($stageNumber+1); $i2++)
            {
              $forInsert = "INSERT INTO ID_$i1 (world, stage, unlocked, stars, highscore,
                                               bestTime, loot_chests, loot_shards)
                           VALUES ('$world', '$i2', '0', '0', '0','0','0','0')";
              if (mysqli_query($connection, $forInsert))
              {
                echo "ROW $i2/$stageNumber for table ID_$i1 created.". '<br />';
              }
              else {
                echo "ERROR: ROW $i2/$stageNumber for table ID_$i1 failed.". '<br />';
              }
            }
            echo "$i1/$userCount done\r\n";

          }
          echo "SUCCESS: All existing tables should be updated with the requested world. (World=$world, $stageNumber stages)". '<br />';

          $newWorldCount = $world;
          $newStages = $stageArray . ":$stageNumber";
          $masterupdate = "UPDATE mastertable
                          SET stageArray='$newStages', worldCount=$newWorldCount
                          ";

          if (mysqli_query($connection, $masterupdate))
          {
            echo "SUCCESS: Required data updated to mastertable";
          }
          else{
            die ("ERROR: mastertable update failed". '<br />' . mysqli_error($connection));
          }


   mysqli_close($connection);
?>
