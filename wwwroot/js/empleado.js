const empleadosUrl = "https://stockapibackend-demo.onrender.com/api/empleado";

window.onload = function () {
  cargarEmpleados();

  const form = document.getElementById("empleado-form");
  if (form) {
    form.onsubmit = manejarFormularioEmpleado;
  }

  const btnCancelar = document.getElementById("btn-cancelar");
  if (btnCancelar) {
    btnCancelar.onclick = () => {
      form.reset();
      document.getElementById("btn-guardar").innerText = "Agregar";
      btnCancelar.classList.add("d-none");
      document.getElementById("empleado-id").value = "";
    };
  }
};

async function cargarEmpleados() {
  const res = await fetch(empleadosUrl, { cache: "no-store" });
  const empleados = await res.json();
  const tbody = document.getElementById("tabla-empleados");
  if (!tbody) return;
  tbody.innerHTML = "";
  empleados.forEach((e) => {
    tbody.innerHTML += `
      <tr>
        <td>${e.nombreyApellido}</td>
        <td>${e.area}</td>
        <td>$${e.sueldo.toFixed(2)}</td>
        <td>
          <button class="btn btn-sm btn-warning me-1" onclick="editarEmpleado(${e.id}, '${e.nombreyApellido}', '${e.area}', ${e.sueldo})">Editar</button>
          <button class="btn btn-sm btn-danger" onclick="eliminarEmpleado(${e.id})">Eliminar</button>
        </td>
      </tr>
    `;
  });
}

window.editarEmpleado = function (id, nombreyApellido, area, sueldo) {
  document.getElementById("empleado-id").value = id;
  document.getElementById("nombre-apellido").value = nombreyApellido;
  document.getElementById("area").value = area;
  document.getElementById("sueldo").value = sueldo;
  document.getElementById("btn-guardar").innerText = "Actualizar";
  document.getElementById("btn-cancelar").classList.remove("d-none");
};

window.eliminarEmpleado = async function (id) {
  if (confirm("Â¿Seguro que quieres eliminar este empleado?")) {
    await fetch(`${empleadosUrl}/${id}`, { method: "DELETE" });
    mostrarMensaje("Empleado eliminado.", "danger");
    cargarEmpleados();
  }
};

async function manejarFormularioEmpleado(e) {
  e.preventDefault();
  const id = document.getElementById("empleado-id").value;
  const nombreyApellido = document.getElementById("nombre-apellido").value;
  const area = document.getElementById("area").value;
  const sueldo = parseFloat(document.getElementById("sueldo").value);

  const body = {
    id: id ? parseInt(id) : 0,
    nombreyApellido: nombreyApellido,
    area: area,
    sueldo: sueldo
  };

  if (id) {
    await fetch(`${empleadosUrl}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    });
    mostrarMensaje("Empleado actualizado correctamente.", "success");
  } else {
    await fetch(empleadosUrl, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(body),
    });
    mostrarMensaje("Empleado agregado correctamente.", "success");
  }

  document.getElementById("empleado-form").reset();
  document.getElementById("btn-guardar").innerText = "Agregar";
  document.getElementById("btn-cancelar").classList.add("d-none");
  document.getElementById("empleado-id").value = "";
  cargarEmpleados();
}

function mostrarMensaje(texto, tipo) {
  const div = document.getElementById("mensaje");
  if (!div) return;
  div.innerHTML = `<div class="alert alert-${tipo}">${texto}</div>`;
  setTimeout(() => (div.innerHTML = ""), 2000);
}

function logout() {
  // Limpiar almacenamiento
  sessionStorage.clear();
  localStorage.clear();

  // Redireccionar forzado y evitar cache
  window.location.replace("../index.html");

  // Prevenir el uso del botón Atrás
  history.pushState(null, "", "../index.html");
  window.addEventListener("popstate", function () {
    history.go(1);
  });
}
