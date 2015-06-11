$(function(){
 $(".img2").click(function(){
	 $(".img1").attr("src","images/9.png");
	 $(this).attr("src","images/12.png");
	 });
 $(".img1").click(function(){
	 $(".img2").attr("src","images/10.png");
	 $(this).attr("src","images/11.png");
	 });
 $(".shijian li").click(function(){
	 $(this).removeClass("bg2").addClass("bg1").siblings().removeClass("bg1").addClass("bg2");
	 $(this).children(".tian").addClass("bg1");
	 $(this).siblings().children(".tian").removeClass("bg1");
	  
	 });
})
