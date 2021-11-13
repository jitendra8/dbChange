// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(() => {
    let connection = new signalR.HubConnectionBuilder().withUrl("/signalServer").build()

    connection.start()

    connection.on("refreshPersons", function () {
        loadData()
    })

    loadData();

    function loadData() {
        var tr = ''

        $.ajax({          
            url: '/Home/GetPersons',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr = tr + `<tr>
                        <td>${v.id}</td>
                        <td>${v.firstName}</td>
                        <td>${v.lastName}</td>
                        <td>${v.gender}</td>
                        <td>${v.salary}</td>
                    </tr>`
                })

                $("#tableBody").html(tr)
            },
            error: (error) => {
                console.log(error)
            }
        })
    }
})
