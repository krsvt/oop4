var data;

const backend = "http://localhost:5000";
const tree = backend + "/api/tree";
const person = backend + "/api/person";
const union = backend + "/api/union";

function renderTable(persons) {
  const tableBody = document.querySelector("#personTable tbody");

  tableBody.innerHTML = "";

  persons.forEach(person => {
    const row = document.createElement("tr");

    row.innerHTML = `
          <td>${person.id}</td>
          <td>${person.name}</td>
          <td>${person.gender == 0 ? 'male' : 'female'}</td>
          <td>${new Date(person.birthDate).toLocaleDateString()}</td>
          <td>${person.deathDate ? new Date(person.deathDate).toLocaleDateString() : "N/A"}</td>
          <td><button class="show-relatives" data-id="${person.id}">Show Relatives</button></td>
        `;

    tableBody.appendChild(row);

    document.querySelectorAll('.show-relatives').forEach(button => {
      button.addEventListener('click', () => {
        const personId = button.getAttribute('data-id');
        console.log("p " + personId);
        fetchRelatives(personId);
      });
    });

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
    let p = await getAllPersons()
    renderTable(p);

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
    updateTree();
  } catch (error) {
    console.error("Ошибка при отправке данных:", error);
  }
}

async function fetchRelatives(personId) {
  try {
    const response = await fetch(`${person}/${personId}/relatives`, {
      method: 'GET',
      headers: {
        'Accept': 'application/json',
      },
    });

    if (!response.ok) {
      throw new Error(`Failed to fetch relatives for person ID: ${personId}`);
    }

    const d = await response.json();
    renderTable(d.relatives);

    document.getElementById('showAllButton').style.display = 'block';
    document.getElementById('showingText').style.display = 'block';
    document.getElementById('showingText').innerText = `Showing relatives of id=${d.personId}`
  } catch (error) {
    console.error('Error fetching relatives:', error);
  }
}

document.addEventListener("DOMContentLoaded", async () => {
  await updateTree();

  const deleteButton = document.getElementById("deleteButton");

  deleteButton.addEventListener("click", async () => {
    await deleteTree();
    await updateTree();
  });

  document.getElementById("addPersonButton").addEventListener("click", async () => {
    await addPerson();
  });

  document.getElementById("createUnionButton").addEventListener("click", async () => {
    const partner1Input = document.getElementById('partner1Input').value.trim();
    const partner2Input = document.getElementById('partner2Input').value.trim();
    const childrenInput = document.getElementById('childrenInput').value.trim();

    const children = childrenInput.split(',').map(id => id.trim()).filter(Boolean).map(Number);

    const unionData = {
      partner: [Number(partner1Input), Number(partner2Input)],
      children: children,
    };

    const response = await fetch(union, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(unionData),
    })

    if (!response.ok) {
      throw new Error(`Failed to create union`);
    }
    await updateTree();
  });

  document.querySelectorAll('.show-relatives').forEach(button => {
    button.addEventListener('click', () => {
      const personId = button.getAttribute('data-id');
      fetchRelatives(personId);
    });
  });

  document.getElementById('showAllButton').addEventListener('click', () => {
    updateTree();
    document.getElementById('showingText').style.display = 'none';
  });

});
