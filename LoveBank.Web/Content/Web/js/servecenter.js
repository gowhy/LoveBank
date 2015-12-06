$(document).ready(function(){
	/*	数字页码选择	*/
	var i=null; //页码号
	$(".l_serve_changenum").click(function(){
		$(this).addClass("l_serve_changeactive");
		$(this).siblings().removeClass("l_serve_changeactive");
		i=parseInt($(this).html())-1;
		$(".l_serve_pageper").html(($(this).html())+"/"+$(this).parent().find($(".l_serve_changenum")).length)
	})

	/*	回到上页页或者向下一页	*/

	function changelistfooter(i){
		$(".l_serve_changenum").eq(i).addClass("l_serve_changeactive");
		$(".l_serve_changenum").eq(i).siblings().removeClass("l_serve_changeactive");
		$(".l_serve_pageper").html($(".l_serve_changenum").eq(i).html()+"/"+5);

	}

	$(".l_serve_changepage").click(function(){
		
		switch($(this).html()){
			case "首页" :
				i=0;
				changelistfooter(i)
			break;

			case "上一页" :
				if(i<=0){
					i=0;
				}
				else{
					i--;
				}
				changelistfooter(i)
			break;

			case "下一页" :
				if(i>=4){
					i=4
				}
				else{
					i++;
				}
				changelistfooter(i)
			break;

			case "尾页" :
					i=4;
				changelistfooter(i)
			break;							
		}
	})


	/*	向需求详情页面添加信息	*/
	$(".l_serve_introl").click(function(){
		

		$(".l_serve_model").eq(0).hide();
		$(".l_serve_model").eq(1).show();
		$(".l_serve_pubtitle").html($(this).html());
		var getinfo=$(this).parent().find("p").find("span");
		for(var winfo=0;winfo<getinfo.length;winfo++){
			$(".l_serve_w").eq(winfo).html(getinfo.eq(winfo).html());
		}

		if($(".l_serve_w").eq(1).html()=="进行中"){
			$(".l_serve_w").eq(1).addClass("statedis");
			$(".l_serve_w").eq(1).siblings().removeClass("statedis");
		}
	})

	/*		显示评论页面	*/
	$(".l_pubbtn").click(function(){
		
		$(".l_serve_model").eq(2).parent().find($(".l_serve_model")).hide();
		$(".l_serve_model").eq(2).show();
	})


})