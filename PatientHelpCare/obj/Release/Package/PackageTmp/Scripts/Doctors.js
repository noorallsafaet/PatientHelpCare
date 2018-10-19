
$(document).ready(function () {

    $('#Area').hide();
    $('#ddlDhakaArea').hide();
    $('#Location').change(function () { LocationChange(); });
    $('#Area').change(function () { Change2Dhaka(); });
    $("#PhotoLocation").change(function () { readURLImage(this); });
});

function LocationChange()
{
    var Location = $('#Location').val();
    
    if (Location == 'North Dhaka') {
        $('#ddlDhakaArea').fadeOut();
        $('#ddlDhakaArea').find("option").remove();
        $('#Area').fadeIn();
        $('#Area').find("option").remove();
        $('#Area').append('<option value=0>Select One</option>');
        $('#Area').append('<option value=Mirpur>Mirpur</option>');
        $('#Area').append('<option value=Mohammadpur>Mohammadpur</option>');
        $('#Area').append('<option value=Gulshan>Gulshan</option>');
    }
    else if (Location == 'South Dhaka') {
        $('#ddlDhakaArea').fadeOut();
        $('#ddlDhakaArea').find("option").remove();
        $('#Area').fadeIn();
        $('#Area').find("option").remove();
        $('#Area').append('<option value=0>Select One</option>');
        $('#Area').append('<option value=Dhanmondi>Dhanmondi</option>');
        $('#Area').append('<option value=Mogbazar>Mogbazar</option>');
        $('#Area').append('<option value=Polton>Polton</option>');
    }

    else {
        $('#ddlDhakaArea').fadeOut();
        $('#ddlDhakaArea').find("option").remove();
        $('#Area').fadeOut();
        $('#Area').find("option").remove();
    }    
}

function Change2Dhaka()
{
    var DhakaArea = $('#Area').val();
    if (DhakaArea == 'Dhanmondi')
    {
        $('#ddlDhakaArea').fadeIn();
        $('#ddlDhakaArea').find("option").remove();
        $('#ddlDhakaArea').append('<option value=0>Select One</option>');
        $('#ddlDhakaArea').append('<option value=1>1</option>');
        $('#ddlDhakaArea').append('<option value=2>2</option>');
        $('#ddlDhakaArea').append('<option value=3>3</option>');
        $('#ddlDhakaArea').append('<option value=4>4</option>');
        $('#ddlDhakaArea').append('<option value=5>5</option>');
        $('#ddlDhakaArea').append('<option value=8>8</option>');
        $('#ddlDhakaArea').append('<option value=9>9 </option>');
        $('#ddlDhakaArea').append('<option value=10>10</option>');
        $('#ddlDhakaArea').append('<option value=11>11</option>');
        $('#ddlDhakaArea').append('<option value=12>12</option>');
        $('#ddlDhakaArea').append('<option value=25>25</option>');
        $('#ddlDhakaArea').append('<option value=26>26</option>');
        $('#ddlDhakaArea').append('<option value=27>27</option>');
    }

    else if(DhakaArea == 'Mogbazar')
    {
        $('#ddlDhakaArea').fadeIn();
        $('#ddlDhakaArea').find("option").remove();
        $('#ddlDhakaArea').append('<option value=0>Select One</option>');
        $('#ddlDhakaArea').append('<option value=Baily Road>Baily Road</option>');
        $('#ddlDhakaArea').append('<option value=Shiddheswari>Shiddheswari</option>');
        $('#ddlDhakaArea').append('<option value=Malibag>Malibag</option>');
        $('#ddlDhakaArea').append('<option value=Kakrail>Kakrail</option>');
    }

    else if (DhakaArea == 'Polton') {
        $('#ddlDhakaArea').fadeIn();
        $('#ddlDhakaArea').find("option").remove();
        $('#ddlDhakaArea').append('<option value=0>Select One</option>');
        $('#ddlDhakaArea').append('<option value=Purana Polton>Purana Polton</option>');
        $('#ddlDhakaArea').append('<option value=Noya Polton>Noya Polton</option>');
        $('#ddlDhakaArea').append('<option value=Shantinagar>Shantinagar</option>');
    }

    else if (DhakaArea == 'Mirpur') {
        $('#ddlDhakaArea').fadeIn();
        $('#ddlDhakaArea').find("option").remove();
        $('#ddlDhakaArea').append('<option value=0>Select One</option>');
        $('#ddlDhakaArea').append('<option value=1, Dhaka Zoo, Botanical Garden>1, Dhaka Zoo, Botanical Garden</option>');
        $('#ddlDhakaArea').append('<option value=2, Rupnagar, Shialbari>2, Rupnagar, Shialbari</option>');
        $('#ddlDhakaArea').append('<option value=6,7, Pallabi>6,7, Pallabi</option>');
        $('#ddlDhakaArea').append('<option value=13,14,15>13,14,15</option>');
        $('#ddlDhakaArea').append('<option value=10,11>10,11</option>');
        $('#ddlDhakaArea').append('<option value=12, Kalshi>12, Kalshi</option>');
    }

    else if (DhakaArea == 'Mohammadpur') {
        $('#ddlDhakaArea').fadeIn();
        $('#ddlDhakaArea').find("option").remove();
        $('#ddlDhakaArea').append('<option value=0>Select One</option>');
        $('#ddlDhakaArea').append('<option value=Shymoli>Shymoli</option>');
        $('#ddlDhakaArea').append('<option value=Lalmatia>Lalmatia</option>');
        $('#ddlDhakaArea').append('<option value=College Gat>College Gat</option>');
        $('#ddlDhakaArea').append('<option value=Katasur>Katasur</option>');
    }

    else if (DhakaArea == 'Gulshan') {
        $('#ddlDhakaArea').fadeIn();
        $('#ddlDhakaArea').find("option").remove();
        $('#ddlDhakaArea').append('<option value=0>Select One</option>');
        $('#ddlDhakaArea').append('<option value=Gulshan 1,2>Gulshan 1,2</option>');
        $('#ddlDhakaArea').append('<option value=Banani>Banani</option>');
        $('#ddlDhakaArea').append('<option value=Mohakhali, Niketon>Mohakhali, Niketon</option>');
    }
    else
    {
        $('#ddlDhakaArea').fadeOut();
        $('#ddlDhakaArea').find("option").remove();
    }
}

function readURLImage(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        var file = input.files[0];
        //check file type
        var lcl_str_FileType = file.type.toString();
        if ((lcl_str_FileType != "image/png") && (lcl_str_FileType != "image/gif") && (lcl_str_FileType != "image/jpeg") && (lcl_str_FileType != "image/jpg") && (lcl_str_FileType != "image/jpeg")) {
            alert("You can select image type 'jpg,gif,png' only!!!");
            $('#PhotoLocation').val("");
            return;
        }

        reader.onload = function (e) {
            $('#ImageViewer').attr('src', e.target.result).width(100)
                    .height(100);
        }

        reader.readAsDataURL(input.files[0]);
    }
}
