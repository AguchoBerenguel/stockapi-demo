document.getElementById("login-form").addEventListener("submit", async (e) => {
  e.preventDefault();

  const nombreUsuario = document.getElementById("usuario").value;
  const contraseña = document.getElementById("Contraseña").value;

  // Validación local simulada
  if (nombreUsuario === "admin" && contraseña === "admin123") {
    window.location.href = "./pages/Empleados.html"; // Página para admin
  } else if (nombreUsuario === "Empleado" && contraseña === "empleado123") {
    window.location.href = "./pages/GestionProductos.html"; // Página para empleados
  } else {
    document.getElementById("mensaje").textContent = "Usuario o contraseña incorrectos.";
  }
});
