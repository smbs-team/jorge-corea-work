import React from 'react';
import './SelectInput.css';
import Select from 'react-dropdown-select';
import deepEqual from 'deep-equal';

class SelectInput extends React.Component {
  constructor(props) {
    super(props);
    this.state = { options: [], values: [] };
  }

  componentDidMount = () => {
    this.setOptions();
  };

  componentDidUpdate = (prevProps, prevState) => {
    if (
      !deepEqual(this.props.source, prevProps.source) ||
      !deepEqual(this.props.value, prevProps.value)
    ) {
      this.setOptions();
    }
  };

  setOptions = () => {
    let options = this.props.source;
    let values = [];
    this.setState(
      {
        options: options,
      },
      () => {
        let label = this.state.options.find(
          value => value[this.props.sourceValue] == this.props.value
        );
        if (label && label[this.props.sourceValue]) {
          values.push(label);
          this.setState({ placeholder: label.name, values: values });
        } else if (label && label['defaultLocString'] === 'None') {
          this.setState({ placeholder: 'None', values: [label] });
        } else if (!label) {
          this.setState({ placeholder: 'Select one', values: [] });
        }
      }
    );
  };

  render = () => {
    return (
      <div>
        <label htmlFor={this.props.name} className="select-label">
          {this.props.label}
        </label>
        <Select
          id={this.props.name}
          name={this.props.name}
          placeholder={this.state.placeholder}
          options={this.state.options}
          className={
            this.props.classToApply !== undefined
              ? this.props.classToApply
              : 'select-input'
          }
          onChange={value => {
            this.props.onChange({
              target: {
                value: value[0] ? value[0][this.props.sourceValue] : null,
                name: this.props.name,
              },
            });
          }}
          readOnly={this.props.readOnly}
          valueField="defaultLocString"
          sourceValue={this.props.sourceValue}
          labelField={this.props.sourceLabel}
          searchable={false}
          values={this.state.values}
        />
      </div>
    );
  };
}

export default SelectInput;
