 
$(function(){  
 
	 
	 $(".s_right").click(function(){  
     var s=$(this).closest(".tejia").index()-1;
	 var t = $(".tejia:eq("+s+") input");  
        t.val(parseInt(t.val())+1);  
		
    }) 
 
    $(".s_left").click(function(){  
	var s=$(this).closest(".tejia").index()-1;
		var t = $(".tejia:eq("+s+") input");  
	if(t.val()>1)
        t.val(parseInt(t.val())-1)  
    })  
   
})  
 