// index.tsx
/**
 * Copyright (c) King County. All rights reserved.
 * @packageDocumentation
 */
import React, { useState, Fragment, useEffect } from 'react';
import NotificationTimelineItem from './NotificationTimelineItem';
import NotificationsIcon from '@material-ui/icons/Notifications';
import { ItemList } from './NotificationTimelineItem';
import {
  dismissNotification,
  getUserJobNotifications,
} from 'services/JobNotificationServices';
import Loader from 'react-loader-spinner';
import 'assets/notification-timeline/notification-timeline.scss';
import { getDataSet, getUserProject } from 'services/common';
import moment from 'moment';
import { useDebounce } from 'react-use';

const NotificationTimeLine = (): JSX.Element => {
  const [open, setOpen] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(false);
  const [notificationList, setNotificationList] = useState<ItemList[]>([]);

  const fetchNotification = async (): Promise<void> => {
    if (!open) {
      try {
        setLoading(true);
        const notifications = await getUserJobNotifications();
        const notificationList: ItemList[] = [];
        const jobList: ItemList[] = [];
        const format = 'MM/DD/YYYY HH:mm';

        if (notifications) {
          for (const notification of notifications.jobNotifications) {
            if (notification.jobNotificationPayload?.length) {
              let payload = JSON.parse(notification.jobNotificationPayload);
              payload = JSON.parse(payload);
              if (payload.ProjectId) {
                const userProject = await getUserProject(payload.ProjectId);
                if (userProject?.project) {
                  const project = userProject?.project;
                  const projectName = project?.projectName;
                  const dataset = project?.projectDatasets.find(
                    (dataset) => dataset.datasetId === payload.DatasetId
                  );
                  const pp = dataset?.dataset.dependencies?.postProcesses?.find(
                    (pp) => pp.datasetPostProcessId === payload.PostProcessId
                  );
                  notificationList.push({
                    id: notification.jobNotificationId,
                    title: `Project Name: ${projectName}`,
                    description:
                      `Dataset: ${dataset?.dataset.datasetName}` +
                      ' - ' +
                      `PostProcess: ${pp?.postProcessName}`,
                    date: moment(notification.createdTimestamp).format(format),
                    status: notification.jobNotificationType,
                    isJob: false,
                  });
                }
              }
            } else {
              notificationList.push({
                id: notification.jobNotificationId,
                title: notification.jobNotificationText,
                description: notification.jobType,
                date: moment(notification.createdTimestamp).format(format),
                status: notification.jobNotificationType,
                isJob: false,
              });
            }
          }
          console.log(`notifications.pendingJobs`, notifications.pendingJobs);
          if (notifications.pendingJobs.length) {
            for (const notification of notifications.pendingJobs) {
              if (notification.jobPayload.DatasetId) {
                const dataSet = await getDataSet(
                  notification.jobPayload.DatasetId
                );
                if (dataSet) {
                  if (notification.jobPayload.DatasetPostProcessId) {
                    const pp =
                      dataSet.dataset.dependencies?.postProcesses?.find(
                        (pp) =>
                          pp.datasetPostProcessId ===
                          notification.jobPayload.DatasetPostProcessId
                      );
                    if (pp) {
                      console.log(`pp`, pp);
                      jobList.push({
                        id: notification.jobId,
                        title: `Dataset Name: ${dataSet.dataset.datasetName}`,
                        description: `PostProcess: ${pp?.postProcessName}`,
                        status: notification.jobStatus,
                        date: moment().format(format),
                        isJob: true,
                      });
                    } else {
                      jobList.push({
                        id: notification.jobId,
                        title: notification.jobResults,
                        description: notification.jobType,
                        status: notification.jobStatus,
                        date: moment().format(format),
                        isJob: true,
                      });
                    }
                  }
                }
              }
            }
          }
        }
        setNotificationList([...notificationList, ...jobList]);
      } catch (error) {
        console.log(`error`, error);
      } finally {
        setLoading(false);
      }
    }
  };

  useEffect(() => {
    fetchNotification(); //eslint-disable-next-line
  }, []);

  useDebounce(
    (): void => {
      fetchNotification();
    },
    10000,
    [notificationList]
  );

  const tooglePanel = async (): Promise<void> => {
    if (open) fetchNotification();
    setOpen((prevState) => !prevState);
  };

  const onDismissAll = async (): Promise<void> => {
    try {
      await dismissNotification(-1);
      setNotificationList([]);
    } catch (error) {
      console.log(`error`, error);
    }
  };

  const handleDelete = async (id: number | string): Promise<void> => {
    try {
      await dismissNotification(id);
      const newList = notificationList.filter((item) => item.id !== id);
      setNotificationList(newList);
    } catch (error) {
      console.log(`error`, error);
    }
  };

  const renderNotifications = (): JSX.Element | JSX.Element[] => {
    if (!loading) {
      return notificationList.map((item) => {
        return (
          <NotificationTimelineItem
            id={item.id}
            title={item.title}
            description={item.description}
            status={item.status}
            date={item.date}
            handleDelete={handleDelete}
            isJob={item.isJob}
          />
        );
      });
    }
    return (
      <div className="NotificationTimeline-spinner">
        <Loader type="Oval" color="#00BFFF" height={80} width={80} />;
      </div>
    );
  };

  const renderDismissBtn = (): JSX.Element => {
    if (!notificationList.length) return <Fragment />;

    return (
      <button onClick={onDismissAll} className="NotificationTimeline-dismiss">
        Dismiss All
      </button>
    );
  };

  const viewMode = localStorage.getItem('view');

  const renderPopup = (): JSX.Element => {
    if (viewMode === 'model') {
      return (
        <ul className={`NotificationTimeline-list ${!open && 'none'}`}>
          <p className="NotificationTimeline-title">Jobs and Notifications</p>
          {renderDismissBtn()}
          {renderNotifications()}
        </ul>
      );
    }
    return (
      <ul className={`NotificationTimeline-list search ${!open && 'none'}`}>
        <p className="NotificationTimeline-title">Jobs and Notifications</p>
        {renderDismissBtn()}
        {renderNotifications()}
      </ul>
    );
  };

  return (
    <div className="NotificationTimeline">
      <div className={`NotificationTimeline-speech ${!open && 'none'}`} />
      <div className="NotificationTimeline-button" onClick={tooglePanel}>
        <NotificationsIcon />
        <div className="NotificationTimeline-counter">
          {notificationList.length}
        </div>
      </div>
      {renderPopup()}
    </div>
  );
};

export default NotificationTimeLine;
