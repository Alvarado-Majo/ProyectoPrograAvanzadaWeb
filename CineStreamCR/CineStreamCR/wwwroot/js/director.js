(() => {

    const Director = {
        tabla: null,
        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },
        inicializarTabla() {
            this.tabla = $('#tblDirector').DataTable({
                ajax: {
                    url: 'Director/GetDirectors',
                    type: 'GET',
                    dataSrc: 'dato'
                },
                columns: [
                    { data: 'directorId' },
                    { data: 'firstName' },
                    { data: 'lastName' },
                    { data: 'birthDate' },
                    {
                        data: null,
                        title: 'Acciones',
                        orderable: false,
                        render: (data, type, row) => {
                            return `
                                   <button class="btn btn-sm btn-primary editar" data-id="${row.directorId}">Editar</button>
                                   <button class="btn btn-sm btn-danger eliminar" data-id="${row.directorId}">Eliminar</button>
                                    `
                        }
                    }
                ],

                language: {
                    url: 'https://cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
                }

            });
        },
        registrarEventos() {

            $('#btnGuardarDirector').on('click', function () {
                Director.guardarDirector();
            });

            $('#btnEditarDirector').on('click', function () {
                Director.editarDirector();
            });

            $('#tblDirector').on('click', '.editar', function () {
                const id = $(this).data('id');
                Director.cargarDirector(id);
            });

            $('#tblDirector').on('click', '.eliminar', function () {
                const id = $(this).data('id');
                Director.eliminarDirector(id);
            });

        },
        guardarDirector() {
            let form = $('#formCrearDirector');

            if (!form.valid()) { //VALIDAR FORMULARIO
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) {

                    if (respuesta.esCorrecto) {

                        $('#modalCrearDirector').modal('hide');
                        form[0].reset();
                        Director.tabla.ajax.reload();

                        Swal.fire({
                            title: 'Correcto',
                            text: respuesta.mensaje,
                            icon: 'success'
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Incorrecto',
                            text: respuesta.mensaje,
                            icon: 'error'
                        });
                    }

                }


            })
        },


        editarDirector() {
            let form = $('#formEditarDirector');

            if (!form.valid()) { 
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) {

                    if (respuesta.esCorrecto) {

                        $('#modalEditarDirector').modal('hide');
                        form[0].reset();
                        Director.tabla.ajax.reload();

                        Swal.fire({
                            title: 'Correcto',
                            text: respuesta.mensaje,
                            icon: 'success'
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Incorrecto',
                            text: respuesta.mensaje,
                            icon: 'error'
                        });
                    }

                }


            })
        },

        eliminarDirector(id) {

            Swal.fire({
                title: "Estas seguro?",
                text: "No podras revertir esta operacion!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonText: "Si, eliminar",
                cancelButtonText: 'Cancelar'
            }).then((result) => {

                if (result.isConfirmed) {

                    $.ajax({
                        url: `/Director/DeleteDirector?id=${id}`,
                        type: 'POST',
                        success: function (respuesta) {
                            if (respuesta.esCorrecto) {
                                Director.tabla.ajax.reload();
                                Swal.fire({
                                    title: 'Correcto',
                                    text: respuesta.mensaje || 'Director eliminado correctamente',
                                    icon: 'success'
                                });
                            } else {
                                Swal.fire({
                                    title: 'Incorrecto',
                                    text: respuesta.mensaje || 'No se pudo eliminar el director',
                                    icon: 'error'
                                });
                            }
                        },
                        error: function () {
                            Swal.fire({
                                title: 'Error',
                                text: 'Ocurrió un error al intentar eliminar el director',
                                icon: 'error'
                            });
                        }
                    });

                }


            });


        },
        cargarDirector(id) {

            $.get(`/Director/GetDirectorById?id=${id}`, function (resultado) {
                //Espacios, para dividir el proceso
                if (resultado.esCorrecto) {
                    let data = resultado.dato;                 

                    $('#DirectorId').val(data.directorId);      
                    $('#FirstName').val(data.firstName);
                    $('#LastName').val(data.lastName);
                    $('#BirthDate').val(data.birthDate);
                    $('#Biography').val(data.biography);
                    

                    $('#modalEditarDirector').modal('show');
                }
            });
        },



    };



    $(document).ready(() => Director.init());

})(); //Encapsulamos el código para evitar conflictos con otras partes del proyecto
