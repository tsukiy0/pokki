import {
  Button,
  Card as BlueprintCard,
  ControlGroup,
  FormGroup,
  InputGroup,
} from "@blueprintjs/core";
import { CardSet, Card, CardIdRandomizer, CardId } from "@pokki/core";
import React, { useState } from "react";
import { BaseProps } from "./BaseProps";

const CardItem: React.FC<BaseProps<{
  value: Card;
  onClick: (id: CardId) => void;
}>> = ({ value, onClick }) => {
  return (
    <BlueprintCard onClick={() => onClick(value.id)}>
      {value.name}
    </BlueprintCard>
  );
};

export const CardSetInput: React.FC<BaseProps<{
  value: CardSet;
  onChange: (value: CardSet) => void;
}>> = ({ className, value, onChange }) => {
  const [name, setName] = useState("");

  const onAdd = () => {
    onChange(value.addCard(new Card(CardIdRandomizer.random(), name)));
    setName("");
  };

  const onRemove = (id: CardId) => {
    onChange(value.removeCard(id));
  };

  return (
    <BlueprintCard className={className}>
      <FormGroup label="Name" labelFor="name">
        <ControlGroup>
          <InputGroup
            id="name"
            value={name}
            onChange={(e: any) => setName(e.target.value)}
          />
          <Button onClick={onAdd} icon="plus" />
        </ControlGroup>
      </FormGroup>
      {value.items.map((item) => {
        return <CardItem value={item} onClick={onRemove} />;
      })}
    </BlueprintCard>
  );
};
