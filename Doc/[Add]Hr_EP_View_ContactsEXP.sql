SELECT
	*
FROM
	(
		SELECT
			TOP 100 PERCENT /*d.ID AS DivisionID,*/
			部门 = CASE
		WHEN ROW_NUMBER () OVER (
			partition BY d.ID
			ORDER BY
				post.ID
		) = 1 THEN
			d.name
		ELSE
			''
		END,
		d.id AS divisionid,
		post.ID AS PostID,
		post.PostName,
		r.Name AS 房间,
		e.ID AS EmployeeID,
		e.Code AS EmployeeCode,
		e.FullName AS 姓名,
		e.Code AS 员工编号,
		'' AS 传真,
		e.Mobile AS 手机号码,
		e.WorkEmail AS 邮箱
	FROM
		dbo.Hr_Employment_Position AS p
	LEFT OUTER JOIN dbo.Hr_Employment_Division AS d ON d.ID = p.DivisionID
	LEFT OUTER JOIN dbo.Hr_Job_Post AS post ON post.ID = p.JobPostID
	LEFT OUTER JOIN dbo.Hr_Employee AS e ON e.ID = p.EmployeeID
	LEFT OUTER JOIN dbo.Hr_EP_Seat AS s ON e.id = s.EmployeeID
	AND s.EmployeeType = 0
	LEFT OUTER JOIN dbo.Hr_EP_Room AS r ON s.RoomID = r.id
	UNION ALL
		SELECT
			TOP 100 PERCENT 部门 = CASE
		WHEN ROW_NUMBER () OVER (
			--partition BY e.ID
			ORDER BY
				e.ID
		) = 1 THEN
			'未指定部门'
		ELSE
			''
		END,
		99 AS divisionid,
		- 1 AS postid,
		'未指定' AS postname,
		r.Name AS 房间,
		e.ID AS EmployeeID,
		e.Code AS EmployeeCode,
		e.FullName AS 姓名,
		e.Code AS 员工编号,
		'' AS 传真,
		e.Mobile AS 手机号码,
		e.WorkEmail AS 邮箱
	FROM
		dbo.Hr_Employee AS e
	LEFT OUTER JOIN dbo.Hr_EP_Seat AS s ON e.id = s.EmployeeID
	AND s.EmployeeType = 0
	LEFT OUTER JOIN dbo.Hr_EP_Room AS r ON s.RoomID = r.id
	WHERE
		e.id NOT IN (
			SELECT
				employeeid
			FROM
				dbo.Hr_Employment_Position
		)
	ORDER BY
		DivisionID,
		postid,
		房间,
		WorkTelephone
	) AS te
UNION ALL
	SELECT
		*
	FROM
		(
			SELECT
				TOP 100 PERCENT 部门 = CASE
			WHEN ROW_NUMBER () OVER (
				--partition BY i.ID
				ORDER BY
					i.ID
			) = 1 THEN
				'实习生'
			ELSE
				''
			END,
			9998 AS divisionid,
			'' AS PostID,
			'' AS PostName,
			r.Name AS 房间,
			i.id AS EmployeeID,
			i.code AS Employeecode,
			i.FullName AS 姓名,
			i.Code AS 员工编号,
			'' AS 传真,
			i.Mobile AS 手机号码,
			'' AS 邮箱
		FROM
			dbo.hr_intern AS i
		LEFT OUTER JOIN dbo.Hr_EP_Seat AS s ON i.id = s.EmployeeID
		AND s.EmployeeType = 1
		LEFT OUTER JOIN dbo.Hr_EP_Room AS r ON s.RoomID = r.id
		WHERE
			i.id IN (
				SELECT
					employeeid
				FROM
					dbo.hr_ep_seat
				WHERE
					employeetype = 1
			)
		ORDER BY
			房间,
			Mobile
		) AS ti
	UNION ALL
		SELECT
			*
		FROM
			(
				SELECT
					TOP 100 PERCENT 部门 = CASE
				WHEN ROW_NUMBER () OVER (
					--partition BY i.ID
					ORDER BY
						t.ID
				) = 1 THEN
					'临时人员'
				ELSE
					''
				END,
				9999 AS DivisionID,
				'' AS PostID,
				'' AS PostName,
				r.Name AS 房间,
				t.id AS EmployeeID,
				t.code AS employeecode,
				t.FullName AS 姓名,
				t.Code AS 员工编号,
				'' AS 传真,
				t.Mobile AS 手机号码,
				t.WorkEmail AS 邮箱
			FROM
				dbo.Hr_Employee_Temporary AS t
			LEFT OUTER JOIN dbo.Hr_EP_Seat AS s ON t.id = s.EmployeeID
			AND s.EmployeeType = 2
			LEFT OUTER JOIN dbo.Hr_EP_Room AS r ON s.RoomID = r.id
			WHERE
				t.id IN (
					SELECT
						employeeid
					FROM
						dbo.hr_ep_seat
					WHERE
						employeetype = 2
				)
			ORDER BY
				房间,
				WorkTelephone
			) AS tt

