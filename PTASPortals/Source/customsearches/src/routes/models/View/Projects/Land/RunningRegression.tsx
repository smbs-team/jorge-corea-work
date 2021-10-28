/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */

import React from 'react';
import Loading from 'components/Loading';

export const RunningRegression = ({
  runningRegression,
}: {
  runningRegression: boolean;
}): JSX.Element => {
  const renderRunninRegressionLoader = (): JSX.Element => {
    if (runningRegression)
      return (
        <div
          style={{
            zIndex: 1099,
            height: '101%',
            position: 'absolute',
            width: '100%',
            background: '#FFF',
          }}
        >
          <Loading />
        </div>
      );
    return <></>;
  };
  return renderRunninRegressionLoader();
};
