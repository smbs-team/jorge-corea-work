import React from "react";
import { ChipBar as Bar } from "@ptas/react-ui-library";

const ChipBar = () => {
  return (
    <div style={{ width: 440 }}>
      <Bar
        chipData={[
          { id: 1, label: "Mono", isSelected: false },
          { id: 2, label: "Lechuga", isSelected: true },
          { id: 3, label: "Aguacate", isSelected: false },
          { id: 4, label: "Perro", isSelected: false },
          { id: 5, label: "Tigre", isSelected: false },
          { id: 6, label: "Sol", isSelected: false },
          { id: 7, label: "Nube", isSelected: false },
          { id: 8, label: "Papaya", isSelected: false },
          { id: 9, label: "Gato", isSelected: false },
          { id: 10, label: "Hormiga", isSelected: false },
          { id: 11, label: "Smart", isSelected: false },
          { id: 12, label: "Arena", isSelected: false },
          { id: 13, label: "Mar", isSelected: false },
          { id: 14, label: "Confite", isSelected: false }
        ]}
        onChipClick={(chip) => console.log(chip)}
      />
    </div>
  );
};

export default ChipBar;
