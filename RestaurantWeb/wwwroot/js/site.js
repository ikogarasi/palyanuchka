$(function () {
    if ($('#dropdown_content').length > 0) {
        $.ajax({
            type: 'GET',
            url: '/User/Cart/GetCartDetails',
            success: function (response) {
                if (response["data"] != null) {
                    var len = response["data"].length;
                    for (i = 0; i < len; ++i) {
                        var imageUrl = response["data"][i]["product"]["imageURL"];
                        var productName = response["data"][i]["product"]["productName"];
                        var count = response["data"][i]["count"];
                        var price = response["data"][i]["product"]["price"]
                            .toLocaleString('en-US', { style: 'currency', currency: 'USD' });
                        var productContent = `
                                            <div class="row mt-4">
                                                <div class="col-4">
                                                    <img src="${imageUrl}" style="width:100%" />
                                                </div>
                                                <div class="col-8">
                                                    <div class="row">
                                                        <h5 class="text-primary">${productName}</h5>            
                                                    </div>
                                                    <div class="row">
                                                        <div class="col">
                                                            <p class="text-secondary">Count: ${count}</p>
                                                        </div>
                                                        <div class="col">
                                                            <p class="text-black text-end">${price}</p>
                                                        </div>   
                                                    </div>
                                                </div>
                                            </div>
                                        `;
                        if (i < len - 1)
                            productContent += '\n<hr />'

                        $('#dropdown_content').append(productContent);
                    }
                }
                else {
                    $('#dropdown_content').append(
                        `<div class="col" style="text-align:center">
                            <span>Shopping cart is empty.</span>                         
                        </div>                        
                        `
                    )
                    document.getElementById('dropdown_divider').remove();
                    document.getElementById('buybtn').remove();
                }
            }
        });
    }
});