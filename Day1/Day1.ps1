
# Part 1
Write-Host "Part 1 :"
$values = Get-Content -LiteralPath "C:\Users\richard\source\repos\AdventOfCode2020\Day1\Day1.txt"
$found = $false;
For($i=0;($i -lt $values.Count-1) -and ($found -ne $true);$i++){
    For($j=$i+1;($j -lt $values.Count) -and ($found -ne $true);$j++){
        $sum = [int]$values[$i]+[int]$values[$j]
        if($sum -eq 2020){ 
            Write-Host "Values found at lines " ($i+1) " and " ($j+1) " and their product is :" ([int]$values[$i]*[int]$values[$j]).ToString();
            $found = $true
        }
    }
}
Write-Host "File parsed for sum of 2 values !"

$found = $false
# Part 2
For($i=0;($i -lt $values.Count-2) -and ($found -ne $true);$i++){
    For($j=$i+1;($j -lt $values.Count-1) -and ($found -ne $true);$j++){
        For($k=$j+1;($k -lt $values.Count) -and ($found -ne $true);$k++)
        {
            $sum = [int]$values[$i]+[int]$values[$j]+[int]$values[$k]
            if($sum -eq 2020){ 
            Write-Host "Values found at lines " ($i+1) "," ($k+1) " and " ($j+1) " and their product is :" ([int]$values[$i]*[int]$values[$j]*[int]$values[$k]).ToString();
            $found = $true
            }
        }
    }
}
Write-Host "File parsed for sum of 3 values!"