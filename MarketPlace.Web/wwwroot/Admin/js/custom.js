﻿function ShowMessage(title, text, theme) {
    window.createNotification({
        closeOnClick: true,
        displayCloseButton: false,
        positionClass: 'nfc-bottom-right',
        showDuration: 4000,
        theme: theme !== '' ? theme : 'success'
    })({
        title: title !== '' ? title : 'اعلان',
        message: decodeURI(text)
    });
}

$(document).ready(function () {
    var editors = $("[ckeditor]");
    if (editors.length > 0) {
        $.getScript('/js/ckeditor.js', function () {
            $(editors).each(function (index, value) {
                var id = $(value).attr('ckeditor');
                ClassicEditor.create(document.querySelector('[ckeditor="' + id + '"]'),
                    {
                        toolbar: {
                            items: [
                                'heading',
                                '|',
                                'bold',
                                'italic',
                                'link',
                                '|',
                                'fontSize',
                                'fontColor',
                                '|',
                                'imageUpload',
                                'blockQuote',
                                'insertTable',
                                'undo',
                                'redo',
                                'codeBlock'
                            ]
                        },
                        language: 'fa',
                        table: {
                            contentToolbar: [
                                'tableColumn',
                                'tableRow',
                                'mergeTableCells'
                            ]
                        },
                        licenseKey: '',
                        simpleUpload: {
                            // The URL that the images are uploaded to.
                            uploadUrl: '/Uploader/UploadImage'
                        }

                    })
                    .then(editor => {
                        window.editor = editor;
                    }).catch(err => {
                        console.error(err);
                    });
            });
        });
    }
});

function FillPageId(pageId) {
    $("#PageId").val(pageId);
    $("#filter-form").submit();
}

$('[ajax-url-button]').on('click', function (e) {
    e.preventDefault();
    var url = $(this).attr('href');
    var itemId = $(this).attr('ajax-url-button');
    swal({
        title: 'اخطار',
        text: "آیا از انجام عملیات مورد نظر اطمینان دارید؟",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "بله",
        cancelButtonText: "خیر",
        closeOnConfirm: false,
        closeOnCancel: false
    }).then((result) => {
        if (result.value) {
            $.get(url).then(result => {
                if (result.status === 'Success') {
                    ShowMessage('موفقیت', result.message);
                    $('#ajax-url-item-' + itemId).hide(1500);
                }
            });
        } else if (result.dismiss === swal.DismissReason.cancel) {
            swal('اعلام', 'عملیات لغو شد', 'error');
        }
    });
});
function OnSuccessRejectItem(res) {
    if (res.status === 'Success') {
        ShowMessage('اعلان موفقیت', res.message);
        $('#ajax-url-item-' + res.data.id).hide(300);
        $('#reject-modal-' + res.data.id).modal('toggle');
        $('#reject-modal-' + res.data.id).modal().hide();
        $('.close').click();
    }
}

$("[main_category_checkbox]").on('change',
    function (e) {
        var isChecked = $(this).is(':checked');
        var selectedCategoryId = $(this).attr('main_category_checkbox');
        console.log(selectedCategoryId);
        if (isChecked) {
            $('#sub_categories_' + selectedCategoryId).slideDown(300);
        } else {
            $('#sub_categories_' + selectedCategoryId).slideUp(300);
            $('[parent-category-id="' + selectedCategoryId + '"]').prop('checked', false);
        }
    });
//$('#add_color_button').on('click',
//    function (e) {
//        e.preventDefault();
//        var colorName = $('#product_color_name_input').val();
//        var colorPrice = $('#product_color_price_input').val();
//        if (colorName != '' && colorPrice != '') {
//            var currentColorsCount = $('#list_of_product_colors tr');
//            var index = currentColorsCount.length;
//            var colorNameNode = `<input type="hidden" value="${colorName}"  name="ProductColors[${index}].ColorName" coler-name-hidden-index="${colorName}-${colorPrice}" >`;
//            var colorPriceNode = `<input type="hidden" value="${colorPrice}"  name="ProductColors[${index}].Price"  coler-price-hidden-index="${colorName}-${colorPrice}" >`;
//            $('#create_product_form').append(colorNameNode);
//            $('#create_product_form').append(colorPriceNode);

//            var colorTableNode = `<tr coler-item-item="${colorName}-${colorPrice}"> <td> ${colorName} </td>  <td> ${colorPrice} </td>  <td> <a class="btn btn-danger" onclick="removeProductColer('${colorName}-${colorPrice}')" >حذف</a> </td>  </tr>`;
//            $('#list_of_product_colors').append(colorTableNode);

//            $('#product_color_name_input').val('');
//            $('#product_color_price_input').val('');
//        } else {
//            ShowMessage("اخطار", "اطفا رنگ را درست وارد کنید", "warning")
//        }
//    });

//function removeProductColer(index2) {
//    $('[coler-item-item="' + index2 + '"]').remove();
//    $('[coler-name-hidden-index="' + index2 + '"]').remove();
//    $('[coler-price-hidden-index="' + index2 + '"]').remove();
//}