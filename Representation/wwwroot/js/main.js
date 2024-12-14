var data;

const backend = "http://localhost:5000";
const tree = backend + "/api/tree";
const person = backend + "/api/person";
const union = backend + "/api/union";

function renderTable(persons) {
  const tableBody = document.querySelector("#personTable tbody");
  const partnersSelect = document.getElementById("partners");
  const childrenSelect = document.getElementById("children");

  tableBody.innerHTML = "";
  partnersSelect.innerHTML = "";
  childrenSelect.innerHTML = "";

  persons.forEach(person => {
    const row = document.createElement("tr");

    row.innerHTML = `
          <td>${person.id}</td>
          <td>${person.name}</td>
          <td>${person.gender == 0 ? 'male' : 'female'}</td>
          <td>${new Date(person.birthDate).toLocaleDateString()}</td>
          <td>${person.deathDate ? new Date(person.deathDate).toLocaleDateString() : "N/A"}</td>
        `;

    tableBody.appendChild(row);
    const option = `<option value="${person.id}">${person.name}</option>`;
    partnersSelect.innerHTML += option;
    childrenSelect.innerHTML += option;

  });
}

async function deleteTree() {
  const response = await fetch(tree, {
    method: 'DELETE',
  });

  if (response.status === 204) {
    alert('Tree deleted successfully!');
  } else {
    const error = await response.json();
    alert(`Failed to delete tree: ${error.detail || response.status}`);
  }
}

async function getAllPersons() {
  const response = await fetch(person);
  if (!response.ok) {
    throw new Error(`HTTP error! status: ${response.status}`);
  }

  return await response.json();
}

async function updateTree() {
  try {
    d3.select("#tree").select("svg").remove();
    const response = await fetch(tree);
    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    data = await response.json();

    if (!data || !data.persons || !data.unions) {
      throw new Error("Ошибка в структуре данных");
    }

    const svg = d3
      .select("#tree")
      .append("svg")
      .attr("width", document.body.offsetWidth / 2)
      .attr("height", document.documentElement.clientHeight);

    let FT = new FamilyTree(data, svg);
    FT.draw();
    renderTable(await getAllPersons());
  } catch (error) {
    console.error("Ошибка загрузки данных или рендера дерева:", error);
  }

}

async function addPerson() {
  try {
    const name = document.getElementById("name").value;
    const gender = parseInt(document.getElementById("gender").value, 10);
    const birthDate = document.getElementById("birthDate").value + "T00:00:00Z";
    const deathDate = document.getElementById("deathDate").value;
    const d = deathDate ? deathDate + "T00:00:00Z" : null;

    // Create person object
    const personData = {
      name: name,
      gender: gender,
      birthDate: birthDate,
      deathDate: d
    };

    const response = await fetch(person, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(personData),
    });

    if (!response.ok) {
      throw new Error(`HTTP error! Status: ${response.status}`);
    }

    const result = await response.json();
    console.log("Ответ от сервера:", result);
    updateTree();
  } catch (error) {
    console.error("Ошибка при отправке данных:", error);
  }
}

document.addEventListener("DOMContentLoaded", async () => {
  updateTree();

  const deleteButton = document.getElementById("deleteButton");

  deleteButton.addEventListener("click", async () => {
    await deleteTree();
    await updateTree();
  });

  document.getElementById("addPersonButton").addEventListener("click", async () => {
    await addPerson();
  });

  document.getElementById("createUnionButton").addEventListener("click", async () => {
    const partnersSelect = document.getElementById("partners");
    const childrenSelect = document.getElementById("children");
    const selectedPartners = Array.from(partnersSelect.selectedOptions).map(o => parseInt(o.value));
    const selectedChildren = Array.from(childrenSelect.selectedOptions).map(o => parseInt(o.value));

    const unionData = {
      partner: selectedPartners,
      children: selectedChildren
    };

    try {
      await fetch(union, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(unionData)
      });
      await updateTree();
    } catch (error) {
      console.error("Error creating union:", error);
      alert("Failed to create union. Please check the console for more details.");
    }
  });

});
