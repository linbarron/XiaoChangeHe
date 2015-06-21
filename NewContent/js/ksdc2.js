$(function(){  
	 $(".s_right").click(function(){ 
	 var t = $(this).siblings("input");  
        t.val(parseInt(t.val())+1);  
		
    }) 
 
    $(".s_left").click(function(){  
	 var t = $(this).siblings("input");  
	if(t.val()>1)
        t.val(parseInt(t.val())-1)  
    })  
   
 //上面导航栏切换效果   
    
	 $(".top_list ul li").click(function(){
	  
     $(this).addClass("bg1").siblings().removeClass("bg1");
	 var s=$(this).index();
	 
	 $(".menu>ul.level1:eq("+s+")").show().siblings().hide();
	 $(".menu>ul.level1:eq("+s+")>li:eq(0)").addClass("bg2");
	 $(".menu>ul.level1:eq("+s+") .level2:eq(0)").show();
});
     $(".level1 li").click(function(){
		
		 $(this).addClass("bg2").siblings().removeClass("bg2");
		 $(this).next("ul").show();
	 $(this).siblings().next("ul").hide();
		
		 });
});