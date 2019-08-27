<html>
	<body>
		<form method="post" action="index.php" style="border:1px solid blue;width:50%;margin:0 auto;text-align:center;">
			<div>
				<label>Username:</label>
				<input name="name" type="text" placeholder="Username">
				<label>Password:</label>
				<input name="password"  placeholder="Password" type="password">
				<button type="submit" name="submit" style="width:100px;display:block;margin:0 auto;">Login</button>
			</div>
		</form>
	</body>
</html>
<?php 
	error_reporting(E_ALL ^ E_NOTICE);
	session_start();
	if($_POST['name'] == "username" && $_POST['password'] == "password")
	{
		$_SESSION['auth'] = 412;
		$_SESSION["password"] = $_POST['password'];
		header('Location: patcher.php');
		exit();
	}
?>