 
$(function(){  
	 $(".s_right").click(function(){ 
 
     var s=$(this).closest(".content2").index()-3;
	 var t = $(".content2:eq("+s+") input");  
        t.val(parseInt(t.val())+1);  
		
    }) 
 
    $(".s_left").click(function(){  
	var s=$(this).closest(".content2").index()-3;
		var t = $(".content2:eq("+s+") input");  
	if(t.val()>1)
        t.val(parseInt(t.val())-1)  
    })  
   
   
   $(".top_list ul li").click(function(){
	  $(this).addClass("bg1").siblings().removeClass("bg1");
	   });
 $(".left_list ol li").click(function(){
 
	  $(this).addClass("bg2").siblings().removeClass("bg2");
	   });
   
})  
 