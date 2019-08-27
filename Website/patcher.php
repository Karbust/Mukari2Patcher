<?php
session_start();
if(!isset($_SESSION['auth']) || $_SESSION['auth'] != 412 || !isset($_SESSION["password"]) || empty($_SESSION["password"]))
{
	header('Location: index.php');
	exit();
}
?>
<!DOCTYPE html>
<html>
	<head>
	</head>
	<body>
		<form method="POST" action=''>
			<div style="font-size:23px;"> <i class="cloud icon"></i> Update files</div>
			<textarea rows="10" cols="75"><?php $data = file_get_contents('patchlist.txt'); echo $data; ?></textarea>	
			<br>
			<button type="submit" name="listupdate"><b>Generate file list</b></button>
		</form>
	</body>
</html>
<?php
if (isset($_POST['listupdate']))
{
	$rii = new RecursiveIteratorIterator(new RecursiveDirectoryIterator('client'));
	$files = array();
	foreach ($rii as $file) 
	{
		if($file->isDir()){
			continue;
		
		$files[] = $file->getPathname();
	}
	$end = '';
	foreach ($files as $file) 
	{
		global $end;
		$end .= str_replace("client\\", "", $file) . " " . md5_file($file) . " " . filesize($file) . "\r\n";
	}
	// Write the contents back to the file

	file_put_contents('patchlist.txt', $end);
	$end = '';
	
	header("Refresh:0");

} 
?>