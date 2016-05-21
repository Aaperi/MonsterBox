<?php
          // Adds next stage to pre-existing, given, world.
          // (Accounts for all tables and further table creation)
          // obviously all stored data will be empty, regarding this newly created stage.
          // eg. if you request next stage for world 2 and world 2 has 5 stages currently,
          // this script will create stage #6 to world 2.

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
            $world      = mysqli_real_escape_string($connection, $_POST['world']);
          }
          else
          {
           echo "FAILURE: No POST";
           exit();
          }

          // script start
          // mastertable search
          $select = "SELECT * FROM mastertable
                    WHERE identification=1";
          $select_query = mysqli_query($connection, $select);
          if (!$select_query)
          {
            die("ERROR: mastertable doesn't exist");
          }
          $usercount = mysqli_fetch_array(mysqli_query($connection,$select))['usercount'];
          $stageArray= mysqli_fetch_array(mysqli_query($connection,$select))['stageArray'];
          $worldCount= mysqli_fetch_array(mysqli_query($connection,$select))['worldCount'];
          $stages = explode(":", $stageArray);


          // check for viable world
          if ($world > $worldCount)
          {
            die ("FAILURE: Given world doesn't exist.");
          }
          elseif ($world <=0)
          {
            die ("FAILURE: Given world is not positive. (Smaller or equal to 0)");
          }

          // next stage in line...
          $stage = $stages[$world-1]+1;



          // inserting the item row to each table
          for ($i = 1; $i < ($usercount+1); $i++)
          {
            $forInsert = "INSERT INTO ID_$i (world, stage, unlocked, stars, highscore,
                                             bestTime, loot_chests, loot_shards)
                         VALUES ('$world', '$stage', '0', '0', '0','0','0','0')";
            if (mysqli_query($connection, $forInsert))
            {
              echo "SUCCESS: $i/$usercount done". '<br />';
            }
            else {
              echo ("ERROR?: Inserting into ID_$i failed.". '<br />' . mysqli_error($connection));
            }
          }
          echo "SUCCESS: All tables should be updated with the requested stage. (Stage: $world-$stage)". '<br />';

          $stages[$world-1] = $stage;
          $newStages = implode(":", $stages);
          $masterupdate = "UPDATE mastertable
                          SET stageArray='$newStages'
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
