(() => {

    const Actor = {
        tabla: null,
        init() {
            this.inicializarTabla();
            this.registrarEventos();
        },
        inicializarTabla() {
            this.tabla = $('#tblActor').DataTable({
                ajax: {
                    url: 'Actor/GetActors',
                    type: 'GET',
                    dataSrc: 'dato'
                },
                columns: [
                    { data: 'actorId' },
                    { data: 'firstName' },
                    { data: 'lastName' },
                    { data: 'nationality' },
                    { data: 'birthDate' },
                    {
                        data: null,
                        title: 'Acciones',
                        orderable: false,
                        render: (data, type, row) => {
                            return `
                                   <button class="btn btn-sm btn-primary editar" data-id="${row.actorId}">Editar</button>
                                   <button class="btn btn-sm btn-danger eliminar" data-id="${row.actorId}">Eliminar</button>
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

            $('#btnGuardarActor').on('click', function () {
                Actor.guardarActor();
            });

            $('#btnEditarActor').on('click', function () {
                Actor.editarActor();
            });

            $('#tblActor').on('click', '.editar', function () {
                const id = $(this).data('id');
                Actor.cargarActor(id);
            });

            $('#tblActor').on('click', '.eliminar', function () {
                const id = $(this).data('id');
                Actor.eliminarActor(id);
            });

        },
        guardarActor() {
            let form = $('#formCrearActor');

            if (!form.valid()) { 
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) {

                    if (respuesta.esCorrecto) {

                        $('#modalCrearActor').modal('hide');
                        form[0].reset();
                        Actor.tabla.ajax.reload();

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


        editarActor() {
            let form = $('#formEditarActor');

            if (!form.valid()) { 
                return;
            }

            $.ajax({
                url: form.attr('action'),
                type: 'POST',
                data: form.serialize(),
                success: function (respuesta) {

                    if (respuesta.esCorrecto) {

                        $('#modalEditarActor').modal('hide');
                        form[0].reset();
                        Actor.tabla.ajax.reload();

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

        eliminarActor(id) {

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
                        url: `/Actor/DeleteActor?id=${id}`,
                        type: 'POST',
                        success: function (respuesta) {
                            if (respuesta.esCorrecto) {
                                Actor.tabla.ajax.reload();
                                Swal.fire({
                                    title: 'Correcto',
                                    text: respuesta.mensaje || 'Actor eliminado correctamente',
                                    icon: 'success'
                                });
                            } else {
                                Swal.fire({
                                    title: 'Incorrecto',
                                    text: respuesta.mensaje || 'No se pudo eliminar el actor',
                                    icon: 'error'
                                });
                            }
                        },
                        error: function () {
                            Swal.fire({
                                title: 'Error',
                                text: 'Ocurrió un error al intentar eliminar el actor',
                                icon: 'error'
                            });
                        }
                    });

                }


            });


        },
        cargarActor(id) {

            $.get(`/Actor/GetActorById?id=${id}`, function (resultado) {
                //Espacios, para dividir el proceso
                if (resultado.esCorrecto) {
                    let data = resultado.dato;                 

                    $('#ActorId').val(data.actorId);            
                    $('#FirstName').val(data.firstName);
                    $('#LastName').val(data.lastName);
                    $('#Nationality').val(data.nationality);
                    $('#BirthDate').val(data.birthDate);
                    $('#Biography').val(data.biography);
             

                    $('#modalEditarActor').modal('show');
                }
            });
        },



    };



    $(document).ready(() => Actor.init());

})(); //Encapsulamos el código para evitar conflictos con otras partes del proyecto
