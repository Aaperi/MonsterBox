<?php
        // mastertable creation. Documentation as follows:
        // usercount will start from 0, and +1 is added after each table creation
        // nextID will start from 1, and rise as with above
        // worldCount is the amount of worlds stored currently
        // stageArray tells, in array form, how many stages each world holds

        // ... something to prevent multiple instances of same script from using
        // wrong values, eg. only one create_table.php can access mastertable
        // at any give time?

        // generally "FAILURE" is users fault or very minor thing, and
        // "ERROR" is something more major that shouldn't really happen at all.

        // Currently there's lack of rollback features in case of major failures,
        // and they'll be added in the future as the ideas about this whole thing
        // soldify.
        // I also wonder how well this current system (referring to mastertable)
        // for data works, since it could perhaps break if multiple users at
        // same frame or something like that.

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

        // script start
        // querys for All

        $check_query = mysqli_query($connection, "SELECT * FROM mastertable");

        $create = "CREATE TABLE mastertable
                  (
                    identification INT NOT NULL,
                    usercount INT NOT NULL,
                    nextID INT NOT NULL,
                    worldCount INT NOT NULL,
                    stageArray VARCHAR(50) NOT NULL
                  )";

        $create_query = mysqli_query($connection, $create);
        $insert = "INSERT INTO mastertable (identification, usercount, nextID, worldCount, stageArray)
                  VALUES ('1','0','1','3','5:5:6')";
        $insert_query = mysqli_query($connection, $insert);

        // execution tree and various error messages
        if (empty($check_query))
        {
          if ($create_query)
          {
            if ($insert_query)
            {
              echo "SUCCESS: mastertable creation and row insertion success.";
            }
            else
            {
              die("ERROR: row insertion failed.". '<br />' . mysqli_error($connection));
            }
          }
          else{
            die("ERROR: mastertable creation failed.". '<br />' . mysqli_error($connection));
          }
        }
        else{
          die("FAILURE: mastertable already exists");
        }
      mysqli_close($connection);
?>
