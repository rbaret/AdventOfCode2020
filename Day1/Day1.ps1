
# Part 1
Write-Host "Part 1 :"
$values = Get-Content -LiteralPath "C:\Users\richard\source\repos\AdventOfCode2020\Day1\Day1.txt"
For($i=0;$i -lt $values.Count-1;$i++){
    For($j=$i+1;$j -lt $values.Count;$j++){
        $sum = [int]$values[$i]+[int]$values[$j]
        if($sum -eq 2020){ 
            Write-Host "Values found at lines " ($i+1) " and " ($j+1) " and their product is :" ([int]$values[$i]*[int]$values[$j]).ToString();
            break;
        }
    }
}
Write-Host "File parsed for sum of 2 values !"

# Part 2
For($i=0;$i -lt $values.Count-2;$i++){
    For($j=$i+1;$j -lt $values.Count-1;$j++){
        For($k=$j+1;$k -lt $values.Count;$k++)
        {
            $sum = [int]$values[$i]+[int]$values[$j]+[int]$values[$k]
            if($sum -eq 2020){ 
            Write-Host "Values found at lines " ($i+1) "," ($k+1) " and " ($j+1) " and their product is :" ([int]$values[$i]*[int]$values[$j]*[int]$values[$k]).ToString();
            return;
            }
        }
    }
}
Write-Host "File parsed for sum of 3 values!"