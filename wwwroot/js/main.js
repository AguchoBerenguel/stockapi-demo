const apiUrl = "https://stockapibackend-demo.onrender.com";

// Cargar productos al iniciar
window.onload = cargarProductos;

// Formulario agregar/editar producto
document.getElementById("product-form").onsubmit = async function (e) {
  e.preventDefault();
  const id = document.getElementById("producto-id").value;
  const nombre = document.getElementById("nombre").value;
  const stock = parseInt(document.getElementById("stock").value);
  const precio = parseFloat(document.getElementById("precio").value);

  if (id) {
    // EDITAR
    await fetch(`${apiUrl}/${id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ id, nombre, stock, precio }),
    });
    mostrarMensaje("Producto editado correctamente.", "success");
  } else {
    // AGREGAR
    await fetch(apiUrl, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ nombre, stock, precio }),
    });
    mostrarMensaje("Producto agregado correctamente.", "success");
  }
  document.getElementById("product-form").reset();
  document.getElementById("btn-guardar").innerText = "Agregar";
  document.getElementById("btn-cancelar").classList.add("d-none");
  document.getElementById("producto-id").value = "";
  cargarProductos();
};

// Cargar productos en la tabla
async function cargarProductos() {
  const res = await fetch(apiUrl, { cache: "no-store" });
  const productos = await res.json();
  const tbody = document.getElementById("tabla-productos");
  tbody.innerHTML = "";
  productos.forEach((p) => {
    const esBajoStock = p.stock <= 5 ? "low-stock" : "";
    tbody.innerHTML += `
      <tr class="${esBajoStock}">
        <td>${p.nombre}</td>
        <td>${p.stock}</td>
        <td>$${Number(p.precio).toFixed(2)}</td>
        <td>
          <button class="btn btn-sm btn-warning me-1" onclick="editarProducto(${p.id}, '${p.nombre}', ${p.stock}, ${p.precio})">Editar</button>
          <button class="btn btn-sm btn-danger" onclick="eliminarProducto(${p.id})">Eliminar</button>
        </td>
      </tr>
    `;
  });
}

// Editar producto (llena el form)
window.editarProducto = function (id, nombre, stock, precio) {
  document.getElementById("producto-id").value = id;
  document.getElementById("nombre").value = nombre;
  document.getElementById("stock").value = stock;
  document.getElementById("precio").value = precio;
  document.getElementById("btn-guardar").innerText = "Actualizar";
  document.getElementById("btn-cancelar").classList.remove("d-none");
};

// Cancelar edición
document.getElementById("btn-cancelar").onclick = function () {
  document.getElementById("product-form").reset();
  document.getElementById("btn-guardar").innerText = "Agregar";
  document.getElementById("btn-cancelar").classList.add("d-none");
  document.getElementById("producto-id").value = "";
};

// Eliminar producto
window.eliminarProducto = async function (id) {
  if (confirm("¿Seguro que quieres eliminar este producto?")) {
    await fetch(`${apiUrl}/${id}`, { method: "DELETE" });
    mostrarMensaje("Producto eliminado.", "danger");
    cargarProductos();
  }
};

// Mostrar mensajes temporales
function mostrarMensaje(texto, tipo) {
  const div = document.getElementById("mensaje");
  div.innerHTML = `<div class="alert alert-${tipo}">${texto}</div>`;
  setTimeout(() => (div.innerHTML = ""), 2500);
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
