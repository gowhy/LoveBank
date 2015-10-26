/*----------自动投标-加减号--start----*/
function jiajian(type){
	switch(type){
		case "jia" :
			$("#ten_value").val(parseInt($("#ten_value").val())+50);
		break;
		case "jian" :
			if(parseInt($("#ten_value").val())-50 >= 200)
				$("#ten_value").val(parseInt($("#ten_value").val())-50);
		break;
	}
}
/*----------自动投标-加减号--end----*/


jQuery(function(){

	//理财计算器 yan 20130916
	$("#J_saveSettingBnt").click(function(){

			$("#T_error").html("");
			if($.trim($("#borrowAmount").val())=="" || $("#borrowAmount").val()%1 !=0){
				$.showErr("借款金额必须为正整数!");
				return false;
			}
			if($.trim($("#apr").val())=="" || isNaN($.trim($("#apr").val()))){
				$.showErr("年利率必须为数字类型!");
				return false;
			}
			if($.trim($("#repayTime").val())=="" || isNaN($.trim($("#repayTime").val()))){
				$.showErr("月份格式必须为数字类型!");
				return false;
			}
			if($.trim($("#repayTime").val())>120){
				$.showErr("月份必须在120以内!");
				return false;
			}
			var query = $("#J_calculate_form").serialize();
			$.ajax({
				url:APP_ROOT + "/index.php?ctl=tool&act=ajax_calculate",
				data:query,
				type:"post",
				success:function(result){
					$("#J_calculate_Result").html(result);
					return false;
				}
			});
			return false;
		});

	$("#qq").click(function(){
		var str = "<div class='row'><div class='12u'><div class='6u'><h3>在线客服</h3>"
		str += "<div class='clearfix'><div class='pull-left' style='padding-top:6px;'><a target='_blank' href='http://wpa.qq.com/msgrd?v=3&amp;uin=1736616101&amp;site=qq&amp;menu=yes'><img border='0' src='http://wpa.qq.com/pa?p=2:1736616101:41' alt='点击这里给我发消息' title='点击这里给我发消息' /></a></div>"
		str+="<div class='pull-left f_red' style='margin-left:10px;'>_乐乐</div></div>"
		str += "<div class='clearfix'><div class='pull-left'  style='padding-top:6px;'><a target='_blank' href='http://wpa.qq.com/msgrd?v=3&amp;uin=1736616101&amp;site=qq&amp;menu=yes'><img border='0' src='http://wpa.qq.com/pa?p=2:1736616101:41' alt='点击这里给我发消息' title='点击这里给我发消息' /></a></div>"
		str+="<div class='pull-left f_red' style='margin-left:10px;'>_乐乐</div></div>"
		str+="</div><div class='6u'  style='border-left:1px solid #d0d0d0;padding-left:30px;'><h3>QQ交流群</h3>"
		str+="<strong class='f_red b' style='font-size:20px;'>171696502</strong>"
		str+="<h3>客服电话</h3>"
		str+="<h3><strong class='f_red'>400-851-6606</strong><h3>"

		str+="</div></div></div>"
		$.weeboxs.open(str,{showButton:false,title:"在线客服",width:500,type:'wee'});
	});

	/*--首次投资验证及绑定----*/
	$("#stepVerifyIdCardAndPhone").submit(function(){
		var obj = $(this);

		//增加验证姓名项 Rafael 20130806
		if(!obj.find("#name").hasClass("readonly")){
			if($.trim(obj.find("#name").val())==""){
				$.showErr(LANG.PLEASE_INPUT+LANG.REAL_NAME,function(){
					obj.find("#name").focus();
				});
				return false;
			}
		}

		if(!obj.find("#idno").hasClass("readonly")){
			if($.trim(obj.find("#idno").val())==""){
				$.showErr(LANG.PLEASE_INPUT+LANG.IDNO,function(){
					obj.find("#idno").focus();
				});
				return false;
			}
			if($.trim(obj.find("#idno").val())!=$.trim(obj.find("#idno_re").val())){
				$.showErr(LANG.TWO_ENTER_IDNO_ERROR,function(){
					obj.find("#idno_re").focus();
				});
				return false;
			}
		}


		/*--身份证号验证-- yan 20130915*/
		var num = obj.find("#idno").val();

		//if(isNaN(num)){
		//	$.showErr("输入的身份证号不是数字，请重新输入！"); 
		//	obj.find("#idno").focus();
		//	return false;
		//}
		var len = num.length, re;
			if (len == 15)
				re = new RegExp(/^(\d{6})()?(\d{2})(\d{2})(\d{2})(\d{3})$/);
				else if (len == 18)
				re = new RegExp(/^(\d{6})()?(\d{4})(\d{2})(\d{2})(\d{3})([0-9]|X)$/);
				else
				 {
				 $.showErr("身份证号位数不对！"); 
				obj.find("#idno").focus();
				return false;
				}

				var a = num.match(re);
				if (a != null)
				{
				if (len==15)
				{
				var D = new Date("19"+a[3]+"/"+a[4]+"/"+a[5]);
				var B = D.getYear()==a[3]&&(D.getMonth()+1)==a[4]&&D.getDate()==a[5];
				}

				else
				{
				var D = new Date(a[3]+"/"+a[4]+"/"+a[5]);
				var B = D.getFullYear()==a[3]&&(D.getMonth()+1)==a[4]&&D.getDate()==a[5];
				}
				if (!B) {
				$.showErr("输入的身份证号 "+ a[0] +" <br>里出生日期不对！"); 
				obj.find("#idno").focus();
				return false;
					}
				}
				


		
		if(!$("#J_Vphone").hasClass("readonly")){
		
			if($.trim($("#J_Vphone").val())==""){
				$.showErr(LANG.MOBILE_EMPTY_TIP,function(){
					$("#J_Vphone").focus();
				});
				return false;
			}
			if(!$.checkMobilePhone($("#J_Vphone").val())){
				$.showErr(LANG.FILL_CORRECT_MOBILE_PHONE,function(){
					$("#J_Vphone").focus();
				});
				return false;
			}
			if($.trim(obj.find("#validateCode2").val())==""){
				$.showErr(LANG.PLEASE_INPUT+LANG.VERIFY_CODE,function(){
					obj.find("#validateCode2").focus();
				});
				return false;
			}
			

		}
		
		//手机号验证 -- yan -- 20130915
		 if(!(/^1[3|4|5|8][0-9]\d{4,8}$/.test($("#J_Vphone").val())))
			 { 
	        $.showErr("手机号码有误，请重新输入！"); 
	        obj.find("#validateCode2").focus();
	        return false; 
	    	} 
           
		var query = obj.serialize();
		$.ajax({
			url:APP_ROOT + "/index.php?ctl=deal&act=dobidstepone",
			data:query,
			dataType:"json",
			success:function(result){
				if(result.status==1)
				{
					//alert(result.info);
					location = "http://www.qiandt.com";
				}
				else{
					$.showErr(result.info);
				}
			}
		});
		return false;
	});


/*--自动投标-start--*/
$("#J_autoBidEnable").click(function(){
		var is_effect = 1;
		if($(this).hasClass("open")){
			is_effect  = 0;
		}
		$.ajax({
			url : APP_ROOT + "/index.php?ctl=uc_autobid&act=autoopen&is_effect="+is_effect,
			dataType : "json",
			cache:false,
			success:function(result){
				if(result.status==1){
					window.location.href = window.location.href;
				}
				else{
					$.showErr(result.info);
				}
			}
		});
});
/*--自动投标-end--*/

/*---提现--start----*/
$("#Jcarry_amount").keyup(function(){
		setCarryResult();
	});
	$("#Jcarry_amount").blur(function(){
		setCarryResult()
	});
	$("#Jcarry_bank_id").change(function(){
		if($(this).val()=="other"){
			$("#Jcarry_otherbank").removeClass("hide");
			$("#Jcarry_bankSuggestNote").addClass("f_red");
			$("#Jcarry_bankSuggestNote").html("其他银行的提现时间约为1-2个工作日,建议使用推荐的银行进行提现操作。");
		}
		else{
			$("#Jcarry_otherbank").addClass("hide");
			$("#Jcarry_otherbank").val("");
			$("#Jcarry_bankSuggestNote").removeClass("f_red");
			if($(this).find("option:selected").attr("day")!=undefined)
				$("#Jcarry_bankSuggestNote").html("提现时间约为1-2个工作日。");
			else
				$("#Jcarry_bankSuggestNote").html("提现时间约为1-2个工作日。");
		}
	});
	
	$("#Jcarry_otherbank").change(function(){
		$("#Jcarry_bankSuggestNote").removeClass("f_red");
		if($(this).find("option:selected").attr("day")!=undefined)
			$("#Jcarry_bankSuggestNote").html("提现时间约为1-2个工作日。");
		else
			$("#Jcarry_bankSuggestNote").html("提现时间约为1-2个工作日。");
	});
	$("#Jcarry_From").submit(function(){
		if($.trim($("#Jcarry_amount").val())=="" || !$.checkNumber($("#Jcarry_amount").val()) || parseFloat($("#Jcarry_amount").val())<=0){
			$.showErr(LANG.CARRY_MONEY_NOT_TRUE,function(){
				$("#Jcarry_amount").focus();
			});
			return false;
		}
		if(parseFloat($("#Jcarry_acount_balance_res").val())<0){
			$.showErr(LANG.CARRY_MONEY_NOT_ENOUGHT,function(){
				$("#Jcarry_acount_balance_res").focus();
			});
			return false;
		}
		
		if($("#Jcarry_real_name").val()==""){
			$.showErr("请输入开户名",function(){
				$("#Jcarry_real_name").focus();
			});
			return false;
		}
		if($("#Jcarry_bank_id").val()==""){
			$.showErr(LANG.PLASE_ENTER_CARRY_BANK,function(){
				$("#Jcarry_bank_id").focus();
			});
			return false;
		}
		if($("#Jcarry_bank_id").val()=="other" && $("#Jcarry_otherbank").val()==""){
			$.showErr(LANG.PLASE_ENTER_CARRY_BANK,function(){
				$("#Jcarry_bank_id").focus();
			});
			return false;
		}
		
		if($("#Jcarry_region_lv4").val()==""){
			$.showErr("请选择开户行所在地",function(){
				$("#Jcarry_region_lv4").focus();
			});
			return false;
		}
		
		if($("#Jcarry_bankzone").val()==""){
			$.showErr("请输入开户行网点",function(){
				$("#Jcarry_bankzone").focus();
			});
			return false;
		}
		
		if($.trim($("#Jcarry_bankcard").val())==""){
			$.showErr(LANG.PLASE_ENTER_CARRY_BANK_CODE,function(){
				$("#Jcarry_bankcard").focus();
			});
			return false;
		}
		if($.trim($("#Jcarry_rebankcard").val())==""){
			$.showErr(LANG.PLASE_ENTER_CARRY_CFR_BANK_CODE,function(){
				$("#Jcarry_rebankcard").focus();
			});
			return false;
		}
		if($.trim($("#Jcarry_bankcard").val())!=$.trim($("#Jcarry_rebankcard").val())){
			$.showErr(LANG.TWO_ENTER_CARRY_BANK_CODE_ERROR,function(){
				$("#Jcarry_rebankcard").focus();
			});
			return false;
		}
		return true;
	});
/*---提现--end----*/
})


/*---提现-----*/
function setCarryResult(){
	var carry_amount = 0;
	var total_amount =  parseFloat($("#Jcarry_totalAmount").val());
	if ($.trim($("#Jcarry_amount").val()).length > 0) {
		if ($("#Jcarry_amount").val() == "-") {
			carry_amount = "-0";
		}
		else{
			carry_amount = parseFloat($("#Jcarry_amount").val());
		}
	}
	if(carry_amount < 0){
		$("#Jcarry_balance").html(LANG.CARRY_MONEY_NOT_TRUE);
	}
	else if(carry_amount > total_amount){
		$("#Jcarry_balance").html(LANG.CARRY_MONEY_NOT_ENOUGHT);
	}
	else if(carry_amount == 0){
		$("#Jcarry_balance").html(LANG.MIN_CARRY_MONEY);
	}
	else{
		$("#Jcarry_balance").html("");
	}
	var fee = 0;
	//去掉提现费用   yan  20131007
	/*if(carry_amount>0&&carry_amount < 20000){
		fee = 1;
	}
	if(carry_amount>=20000&&carry_amount < 50000){
		fee = 3;
	}
	if(carry_amount >= 50000){
		fee = 5;
	}*/
	if(carry_amount + fee > total_amount){
		$("#Jcarry_balance").html(LANG.CARRY_MONEY_NOT_ENOUGHT);
	}
	
	$("#Jcarry_fee").html(foramtmoney(fee,2)+" 元");
	var realAmount = carry_amount+fee;
	$("#Jcarry_realAmount").html(foramtmoney(realAmount,2)+" 元");
	var acount_balance = total_amount-carry_amount-fee;
	$("#Jcarry_acount_balance_res").val(foramtmoney(acount_balance,2));
	$("#Jcarry_acount_balance").html(foramtmoney(acount_balance,2)+" 元");
}

//格式化金额
function foramtmoney(price, len)   
{  
   len = len > 0 && len <= 20 ? len : 2;   
   price = parseFloat((price + "").replace(/[^\d\.-]/g, "")).toFixed(len) + "";   
   var l = price.split(".")[0].split("").reverse(),   
   r = price.split(".")[1];   
   t = "";   
   for(i = 0; i < l.length; i ++ )   
   {   
      t += l[i] + ((i + 1) % 3 == 0 && (i + 1) != l.length ? "," : "");   
   }   
   var re = t.split("").reverse().join("") + "." + r;
   return re.replace("-,","-");
} 


	
/*---绑定手机号-----*/

function sendPhoneCode(o, obj){
	if($.trim($(obj).val())==""){
		$.showErr(LANG.VERIFY_MOBILE_EMPTY);
		return false;
	}
	if(!$.checkMobilePhone($(obj).val())){
		$.showErr(LANG.FILL_CORRECT_MOBILE_PHONE);
		return false;
	}
	if(!(/^1[3|4|5|8][0-9]\d{4,8}$/.test($("#J_Vphone").val())))
			 { 
	        $.showErr("手机号码有误，请重新输入！"); 
	        obj.find("#validateCode2").focus();
	        return false; 
	    	} 


	get_verify_code(obj,function(){
			ResetsendPhoneCode(60);
		});
	}
var resetSpcThread = null;
function ResetsendPhoneCode(times){
	clearTimeout(resetSpcThread);
	if(times > 0){
		times -- ;
		$("#reveiveActiveCode").val(LANG.DO_GET+LANG.MOBILE_VERIFY_CODE +" "+ times);
		resetSpcThread = setTimeout("ResetsendPhoneCode("+times+")",1000);
	}
	else{
		$("#reveiveActiveCode").val(LANG.DO_GET+LANG.MOBILE_VERIFY_CODE);
	}
}

function get_verify_code(obj,func)
{
	var user_mobile = $(obj).val();
	var ajaxurl = APP_ROOT+"/index.php?ctl=ajax&act=get_verify_code&user_mobile="+user_mobile;
	$.ajax({ 
		url: ajaxurl,
		dataType: "json",
		success: function(obj){
			if (obj.status) {
				if (func != null) {
					func.call(this);
				}
				$.showSuccess(obj.info);
			}
			else {
				$("#reveiveActiveCode").removeClass("dis_rActcode");
				$.showErr(obj.info);
			}
		},
		error:function(ajaxobj)
		{
//			if(ajaxobj.responseText!='')
//			alert(ajaxobj.responseText);
		}
	});
}

/*验证*/
$.minLength = function(value, length , isByte) {
	var strLength = $.trim(value).length;
	if(isByte)
		strLength = $.getStringLength(value);
		
	return strLength >= length;
};

$.maxLength = function(value, length , isByte) {
	var strLength = $.trim(value).length;
	if(isByte)
		strLength = $.getStringLength(value);
		
	return strLength <= length;
};
$.getStringLength=function(str)
{
	str = $.trim(str);
	
	if(str=="")
		return 0; 
		
	var length=0; 
	for(var i=0;i <str.length;i++) 
	{ 
		if(str.charCodeAt(i)>255)
			length+=2; 
		else
			length++; 
	}
	
	return length;
};

$.checkNumber = function(value){
	if($.trim(value)!='')
		return !isNaN($.trim(value));
	else
		return true;
};

$.checkMobilePhone = function(value){
	if($.trim(value)!='')
		return /^\d{6,}$/i.test($.trim(value));
	else
		return true;
};
$.checkEmail = function(val){
	var reg = /^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/; 
	return reg.test(val);
};


function formSuccess(obj,msg)
{
	if (msg != '') {
		$(obj).parent().find(".hint").css({"color":"#f7f7f7"});
		$(obj).parent().find(".f-input-tip").html("<span class='form_success'>" + msg + "</span>");
	}
	else {
		$(obj).parent().find(".hint").css({"color":"#989898"});
		$(obj).parent().find(".f-input-tip").html("");
	}
}
function formError(obj,msg)
{
		$(obj).parent().find(".hint").css({"color":"red"});
		$(obj).parent().find(".f-input-tip").html("<span class='form_err'>" + msg + "</span>");
	
}


//用于未来扩展的提示正确错误的JS
$.showErr = function(str,func)
{
	$.weeboxs.open(str, {boxid:'fanwe_error_box',contentType:'text',showButton:true, showCancel:false, showOk:true,title:'错误',width:250,type:'wee',onclose:func});

};

$.showSuccess = function(str,func)
{
	$.weeboxs.open(str, {boxid:'fanwe_success_box',contentType:'text',showButton:true, showCancel:false, showOk:true,title:'提示',width:250,type:'wee',onclose:func});
};

/*--guarantee_adv--*/
var timer;
var c_idx = 1;
var total = 0;
var is_has_show = false;
$(document).ready(function(){
	$("#main_adv_box").find("span").each(function(){
		if($.trim($(this).html())!=""){
			if (!is_has_show) {
				$(this).show();
				is_has_show = true;
			}
			total ++;
		}
	});
	if (total > 1) {
		$("#main_adv_ctl li").hide();
		init_main_adv();
		for(i=1;i<=total;i++){
			$("#main_adv_ctl li[rel='"+i+"']").show();
		}
		$("#main_adv_ctl ul").css({"width":35*total+"px"});
	}
	else {
		if(total==0)
			$("#main_adv_box").hide();
		$("#main_adv_ctl").hide();
	}	
});

function init_main_adv()
{
	$("#main_adv_box").find("span[rel='1']").show();
	$("#main_adv_box").find("li[rel='1']").addClass("act");
	
	timer = window.setInterval("auto_play()", 5000);
	$("#main_adv_box").find("li").hover(function(){
		show_current_adv($(this).attr("rel"));		
	});
	
	$("#main_adv_box").hover(function(){
		clearInterval(timer);
	},function(){
		timer = window.setInterval("auto_play()", 5000);
	});
	init_success_play();
}

function auto_play()
{	
	if(c_idx == total)
	{
		c_idx = 1;
	}
	else
	{
		c_idx++;
	}
	show_current_adv(c_idx);
}

function show_current_adv(idx)
{	
	$("#main_adv_box").find("span[rel!='"+idx+"']").hide();
	$("#main_adv_box").find("li").removeClass("act");
	$("#main_adv_box").find("li").find("div div div div").css("background-color","#fff");
	if($("#main_adv_box").find("span[rel='"+idx+"']").css("display")=='none')
	$("#main_adv_box").find("span[rel='"+idx+"']").fadeIn();
	$("#main_adv_box").find("li[rel='"+idx+"']").addClass("act");
	$("#main_adv_box").find("li[rel='"+idx+"']").find("div div div div").css("background-color","#f60");
	c_idx = idx;
	
	
}

function init_success_play(){
	var a = function() {
		this.h = 50,
		this.speed = 50,
		this.ul = $("#examIndex ul"),
		this.timer = null,
		this.isPasue = !1,
		this.isLoop = !0,
		this.play = function() {
			if (this.ul[0] == undefined || this.ul.find("li").length <= 5) return;
			var a = this.ul.find("li:first"),
			b = this.ul.find("li:last"),
			c = document.createElement("li");
			c.style.height = "0px",
			a.before(c);
			var d = b.html(),
			e = 0,
			f = this.h,
			g = this;
			g.timer = setInterval(function() {
				if (g.isPasue) return ! 1;
				c.style.height = e + "px",
				e += 4,
				e >= f && (clearInterval(g.timer), b.remove(), $(c).css("opacity", 0), $(c).html(d).animate({
					opacity: 1
				}), g.isLoop && setTimeout(function() {
					g.play()
				},
				3e3))
			},
			this.speed)
		},
		this.pause = function() {
			this.isPasue = !0
		},
		this.replay = function() {
			this.isPasue = !1
		}
	},
	b = new a;
	b.play(),
	$("#examIndex ul").hover(function() {
		b.pause()
	},
	function() {
		b.replay()
	})
}

