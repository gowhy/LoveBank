

(function ($) {
    $.fn.WebUploaderMVC = function(options) { //定义插件的名称，这里为WebUploaderLoveBank
        var dft = {
            //以下为该插件的属性及其默认值
            Controllers: "/lovebank_ad/", //控制器
            action: "/UpLoadProcess", //action
            pickid: "#filePicker",
            pickid2: "#filePicker2",
            sourceFileList:  new Array() //初始化图片或者文件资源数组 Array对象。是后台的SourceFile类的实力数组
        };
        var ops = $.extend(dft,options);
    

        var pickid = ops.pickid;
        var pickid2 = ops.pickid2;
        var serverUrl = ops.Controllers+ops.action;
        //var $wrap = $('#uploader');
        var $wrap = $(this);
       // 图片容器
       var  $queue = $('<ul class="filelist"></ul>')
           .appendTo($wrap.find('.queueList')),

       // 状态栏，包括进度和控制按钮
       $statusBar = $wrap.find('.statusBar'),

       // 文件总体选择信息。
       $info = $statusBar.find('.info'),

       // 上传按钮
       $upload = $wrap.find('.uploadBtn'),

       // 没选择文件之前的内容。
       $placeHolder = $wrap.find('.placeholder'),

       // 总体进度条
       $progress = $statusBar.find('.progress').hide(),

       // 添加的文件数量
           fileCount = 0,

       // 添加的文件总大小
          fileSize = 0,

       // 优化retina, 在retina下这个值是2
       ratio = window.devicePixelRatio || 1,

       // 缩略图大小
       thumbnailWidth = 110 * ratio,
       thumbnailHeight = 110 * ratio,

       // 可能有pedding, ready, uploading, confirm, done.
       state = 'pedding',

       // 所有文件的进度信息，key为file id
       percentages = {},

       supportTransition = (function () {
           var s = document.createElement('p').style,
               r = 'transition' in s ||
                     'WebkitTransition' in s ||
                     'MozTransition' in s ||
                     'msTransition' in s ||
                     'OTransition' in s;
           s = null;
           return r;
       })(),

       // WebUploader实例
       uploader;

        ///上传成功的资源文件对象
        var sourceFileList = dft.sourceFileList;
        
        // 实例化
        uploader = WebUploader.create({
            pick: {
                //id: '#filePicker',
                //id: "#"+$(this).find(".filePicker").attr("id"),
                id:pickid,
                label: '点击选择文件'
            },
            //dnd: '#uploader .queueList',
            dnd: $(this).selector+ ' .queueList',
            paste: document.body,

            //accept: {
            //    title: 'Images',
            //    extensions: 'gif,jpg,jpeg,bmp,png',
            //    mimeTypes: 'image/*'
            //},

            // swf文件路径
            swf: '/Content/plug-in/webuploader/Uploader.swf',
            server: serverUrl,
            disableGlobalDnd: true,
            //chunked: true,
            //chunkRetry :1,

            fileNumLimit: 10,
            fileSizeLimit: 5000 * 1024 * 1024,    // 200 M
            fileSingleSizeLimit: 1000 * 1024 * 1024    // 50 M
        });

        PageInit();
        function PageInit() {
            $placeHolder.addClass('element-invisible');
            $statusBar.show();
            $queue.parent().addClass('filled');
            //初始化
            //var sourceFileListJsonObj = eval(@Html.Raw( Json.Encode(@Model.SourceFileList)));

            if (sourceFileList) {
                for (var i = 0; i < sourceFileList.length; i++) {


                    var fileEdit = {};
                    fileEdit.id = sourceFileList[i].Id;
                    fileEdit.getStatus = function () { return "" }
                    fileEdit.name = sourceFileList[i].FileName
                    fileEdit.HttpUrl = sourceFileList[i].Domain + sourceFileList[i].Path;
                    //addFile(file)
                    PageInitImg(fileEdit);
                }

                fileCount = sourceFileList.length;//初始化
            }
         

        }

        function PageInitImg(fileEdit) {

            var $li = $('<li id="' + fileEdit.id + '">' +
                     //'<p class="title">' + file.name + '</p>' +
                     '<div class="imgWrap"></div>' +
                     '<p class="progress"><span></span></p>' +
                     '</li>'),

                 $btns = $('<div class="file-panel">' +
                     '<span class="cancel">删除</span>' +

                     '</div>').appendTo($li),
                 $prgress = $li.find('p.progress span'),
                 $wrap = $li.find('div.imgWrap'),
                 $info = $('<p class="error"></p>');



            var filepath = fileEdit.HttpUrl;

          
            var extStart = filepath.lastIndexOf(".");
            var ext = filepath.substring(extStart, filepath.length).toUpperCase();
            if (ext != ".BMP" && ext != ".PNG?" && ext != ".GIF" && ext != ".JPG" && ext != ".JPEG") {
                $wrap.empty().text(fileEdit.name);
            } else {
                var img = $('<img width="110" height="110" src="' + fileEdit.HttpUrl + '">');
                $wrap.empty().append(img);
            }



            $li.on('mouseenter', function () {
                $btns.stop().animate({ height: 30 });
            });

            $li.on('mouseleave', function () {
                $btns.stop().animate({ height: 0 });
            });

            $btns.on('click', 'span', function () {
                var index = $(this).index(),
                    deg;

                switch (index) {
                    case 0:
                        //uploader.removeFile(file);
                        fileCount--;
                        removeFile(fileEdit);
                        //$(this).remove();
                        return;

                    case 1:
                        file.rotation += 90;
                        break;

                    case 2:
                        file.rotation -= 90;
                        break;
                }

                if (supportTransition) {
                    deg = 'rotate(' + file.rotation + 'deg)';
                    $wrap.css({
                        '-webkit-transform': deg,
                        '-mos-transform': deg,
                        '-o-transform': deg,
                        'transform': deg
                    });
                } else {
                    $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');

                }


            });

            $li.appendTo($queue);
        }

        if (!WebUploader.Uploader.support()) {
            alert('Web Uploader 不支持您的浏览器！如果你使用的是IE浏览器，请尝试升级 flash 播放器');
            throw new Error('WebUploader does not support the browser you are using.');
        }

        //     $info = $statusBar.find('.info'),
        //"#"+$(this).find(".filePicker").attr("id"),
        // 添加“添加文件”的按钮，
        uploader.addButton({
            //id: '#filePicker2',
            id: pickid2,
            label: '继续添加'
        });

        // 当有文件添加进来时执行，负责view的创建
        function addFile(file) {
            var $li = $('<li id="' + file.id + '">' +
                    //'<p class="title">' + file.name + '</p>' +
                    '<div class="imgWrap"></div>' +
                    '<p class="progress"><span></span></p>' +
                    '</li>'),

                $btns = $('<div class="file-panel">' +
                    '<span class="cancel">删除</span>' +

                    '</div>').appendTo($li),
                $prgress = $li.find('p.progress span'),
                $wrap = $li.find('div.imgWrap'),
                $info = $('<p class="error"></p>'),

                showError = function (code) {
                    switch (code) {
                        case 'exceed_size':
                            text = '文件大小超出';
                            break;

                        case 'interrupt':
                            text = '上传暂停';
                            break;

                        default:
                            text = '上传失败，请重试';
                            break;
                    }

                    $info.text(text).appendTo($li);
                };

            if (file.getStatus() === 'invalid') {
                showError(file.statusText);
            } else {
                //  lazyload
                $wrap.text('预览中');


                uploader.makeThumb(file, function (error, src) {
                    if (error) {
                        $wrap.text(file.name);
                        //$wrap.text('不能预览');
                        return;
                    }

                    var img = $('<img name="PageImg" src="' + src + '">');
                    $wrap.empty().append(img);
                }, thumbnailWidth, thumbnailHeight);

                percentages[file.id] = [file.size, 0];
                file.rotation = 0;
            }

            file.on('statuschange', function (cur, prev) {
                if (prev === 'progress') {
                    $prgress.hide().width(0);
                } else if (prev === 'queued') {
                    //$li.off('mouseenter mouseleave');
                    //$btns.remove();
                }

                // 成功
                if (cur === 'error' || cur === 'invalid') {
                    console.log(file.statusText);
                    showError(file.statusText);
                    percentages[file.id][1] = 1;
                } else if (cur === 'interrupt') {
                    showError('interrupt');
                } else if (cur === 'queued') {
                    percentages[file.id][1] = 0;
                } else if (cur === 'progress') {
                    $info.remove();
                    $prgress.css('display', 'block');
                } else if (cur === 'complete') {
                    $li.append('<span class="success"></span>');
                }

                //$li.removeClass('state-' + prev).addClass('state-' + cur);
            });

            $li.on('mouseenter', function () {
                $btns.stop().animate({ height: 30 });
            });

            $li.on('mouseleave', function () {
                $btns.stop().animate({ height: 0 });
            });

            $btns.on('click', 'span', function () {
                var index = $(this).index(),
                    deg;

                switch (index) {
                    case 0:
                        uploader.removeFile(file);
                        return;

                    case 1:
                        file.rotation += 90;
                        break;

                    case 2:
                        file.rotation -= 90;
                        break;
                }

                if (supportTransition) {
                    deg = 'rotate(' + file.rotation + 'deg)';
                    $wrap.css({
                        '-webkit-transform': deg,
                        '-mos-transform': deg,
                        '-o-transform': deg,
                        'transform': deg
                    });
                } else {
                    $wrap.css('filter', 'progid:DXImageTransform.Microsoft.BasicImage(rotation=' + (~~((file.rotation / 90) % 4 + 4) % 4) + ')');

                }


            });

            $li.appendTo($queue);
        }



        function updateTotalProgress() {
            var loaded = 0,
                total = 0,
                spans = $progress.children(),
                percent;

            $.each(percentages, function (k, v) {
                total += v[0];
                loaded += v[0] * v[1];
            });

            percent = total ? loaded / total : 0;

            spans.eq(0).text(Math.round(percent * 100) + '%');
            spans.eq(1).css('width', Math.round(percent * 100) + '%');
            updateStatus();
        }

        function updateStatus() {
            var text = '', stats;

            if (state === 'ready') {
                text = '选中' + fileCount + '个文件，共' +
                        WebUploader.formatSize(fileSize) + ' 请选择完成后上传文件完成在点击新增。';
            } else if (state === 'confirm') {
                stats = uploader.getStats();
                if (stats.uploadFailNum) {
                    text = '已成功上传' + stats.successNum + '个文件，' +
                        stats.uploadFailNum + '文件上传失败，<a class="retry" href="#">重新上传</a>失败文件或<a class="ignore" href="#">忽略</a>'
                }

            } else {
                stats = uploader.getStats();
                text = '共' + fileCount + '个文件（' +
                        WebUploader.formatSize(fileSize) +
                        '），已上传' + stats.successNum + '个';

                if (stats.uploadFailNum) {
                    text += '，失败' + stats.uploadFailNum + '个';
                }
            }

            $info.html(text);
        }

        function setState(val) {
            var file, stats;

            if (val === state) {
                return;
            }

            $upload.removeClass('state-' + state);
            $upload.addClass('state-' + val);
            state = val;

            switch (state) {
                case 'pedding':
                    $placeHolder.removeClass('element-invisible');
                    $queue.parent().removeClass('filled');
                    $queue.hide();
                    $statusBar.addClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'ready':
                    $placeHolder.addClass('element-invisible');
                    //$('#filePicker2').removeClass('element-invisible');
                    $queue.parent().addClass('filled');
                    $queue.show();
                    $statusBar.removeClass('element-invisible');
                    uploader.refresh();
                    break;

                case 'uploading':
                    //$('#filePicker2').addClass('element-invisible');
                    $progress.show();
                    $upload.text('上传中').addClass('disabled');
                    break;

                case 'paused':
                    $progress.show();
                    //$upload.text('继续上传');
                    break;

                case 'confirm':
                    $progress.hide();
                    //$upload.text('开始上传').addClass('disabled');
                    $upload.text('开始上传').removeClass('disabled');
                    stats = uploader.getStats();
                    if (stats.successNum && !stats.uploadFailNum) {
                        setState('finish');
                        return;
                    }
                    break;
                case 'finish':
                    stats = uploader.getStats();
                    if (stats.successNum) {
                        alert('上传成功');
                    } else {
                        // 没有成功的图片，重设
                        state = 'done';
                        location.reload();
                    }
                    break;
            }

            updateStatus();
        }

        uploader.onUploadProgress = function (file, percentage) {
            var $li = $('#' + file.id),
                $percent = $li.find('.progress span');

            $percent.css('width', percentage * 100 + '%');
            percentages[file.id][1] = percentage;
            updateTotalProgress();
        };

        uploader.onFileQueued = function (file) {
            fileCount++;
            fileSize += file.size;

            if (fileCount === 1) {
                $placeHolder.addClass('element-invisible');
                $statusBar.show();
            }

            addFile(file);
            setState('ready');
            updateTotalProgress();
        };

        uploader.onFileDequeued = function (file) {
            fileCount--;
            fileSize -= file.size;

            if (!fileCount) {
                setState('pedding');
            }

            removeFile(file);
            updateTotalProgress();

        };


        uploader.on('all', function (type) {
            var stats;
            switch (type) {
                case 'uploadFinished':
                    setState('confirm');
                    break;

                case 'startUpload':
                    setState('uploading');
                    break;

                case 'stopUpload':
                    setState('paused');
                    break;

            }
        });

        uploader.onError = function (code) {
            alert('Eroor: ' + code);
        };

        $upload.on('click', function () {
            if ($(this).hasClass('disabled')) {
                return false;
            }

            if (state === 'ready') {
                uploader.upload();
            } else if (state === 'paused') {
                uploader.upload();
            } else if (state === 'uploading') {
                uploader.stop();
            }
        });


        $info.on('click', '.retry', function () {
            uploader.retry();
        });

        $info.on('click', '.ignore', function () {
            alert('todo');
        });

        $upload.addClass('state-' + state);
        updateTotalProgress();


        // 负责view的销毁
        function removeFile(file) {
            var $li = $('#' + file.id);

            delete percentages[file.id];
            updateTotalProgress();

            //删除上传对象sourceFileList.数组中的对象
            if (sourceFileList != null) {
                for (var i = 0; i < sourceFileList.length; i++) {
                    if (sourceFileList[i] != null && sourceFileList[i].Id == file.id) {

                        sourceFileList.splice(i, 1)//删除对象
                        //sourceFileList[i].State = 1;
                    }
                }
            }

            $li.off().find('.file-panel').off().end().remove();
        }

        // 文件上传成功，给item添加成功class, 用样式标记上传成功。
        uploader.on('uploadSuccess', function (file, response) {

            response.ClientFileId = file.id;//把客户端文件的Id家上，方便在本地区分
            sourceFileList.push(response);

        });

        this.getSourceFileList=  function()
        {
            return sourceFileList;
        }
        this.getfileCount=  function()
        {
            return fileCount;
        }
        this.getfileSize=  function()
        {
            return fileSize;
        }
      
        return this;
    
    }

    $.fn.TreeMvc = function (options) { //定义插件的名称，这里为WebUploaderLoveBank
        var dft = {
          
            Department_List: new Array() //初始化图片或者文件资源数组 Array对象。是后台的SourceFile类的实力数组
        };
        var ops = $.extend(dft, options);
        
        var setting = {
            view: {
                dblClickExpand: false,
                showLine: true,
                selectedMulti: false

            },
            check: {
                enable: true ,
                chkStyle: "radio",
                radioType: "all"
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "Id",
                    pIdKey: "PId",
                  
                    rootPId: ""
                },
                key: {
                    name: "Name",
                    checked: "Ischecked"
                }

            },
            callback: {
                beforeCheck: beforeCheck,
                onCheck: onCheck
            }
        };

        //var zNodes =@(Html.Raw(ViewData["Department_List"].ToString()));

        var zNodes =ops.Department_List;

        function beforeCheck(treeId, treeNode) {

        }
        function onCheck(e, treeId, treeNode) {
            $("#DeptId").val(treeNode.Id);
            $("#CoverCommunity").val(treeNode.Name);
        }
        //$(this).onclick(function () {

        //    $("#menuContent").slideDown("fast");
        //    $("body").bind("mousedown", onBodyDown);
        //});

        $(this).on('click', function () {
            $("#menuContent").slideDown("fast");
            $("body").bind("mousedown", onBodyDown);
        });
        function showMenu() {
            $("#menuContent").slideDown("fast");
            $("body").bind("mousedown", onBodyDown);
        }
        function hideMenu() {
            $("#menuContent").fadeOut("fast");
            $("body").unbind("mousedown", onBodyDown);
        }
        function onBodyDown(event) {
            if (!(event.target.id == "menuBtn" || event.target.id == "menuContent" || $(event.target).parents("#menuContent").length>0)) {
                hideMenu();
            }
        }


        $(document).ready(function () {
            $.fn.zTree.init($("#ztree"), setting, zNodes);
        });

    }
})(jQuery);


