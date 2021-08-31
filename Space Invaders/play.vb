﻿
Imports System.Drawing.Text
Public Class Form2
    'variable setting
    Dim numofshots As Integer = 5
    Dim projArray(numofshots) As PictureBox 'an array of bullets (5 picture boxes)
    Dim projNum As Integer = 0
    Dim projOnScreen(numofshots) As Boolean
    Dim playerRight As Boolean = False
    Dim playerLeft As Boolean = False
    Dim maxEnemyNum As Integer = 5
    Dim enemyArray(maxEnemyNum) As PictureBox
    Dim enemyOnScreen(maxEnemyNum) As Boolean
    Dim shooting As Boolean = False
    Dim enemyMoveRight As Boolean = True
    'variables for health
    Dim hearts As Integer = 3
    'variables for getting hit
    Dim playerhit As Boolean = False
    Dim Gameoverboolean As Boolean = False

    'powerups | attack powerups last 10 seconds
    Dim normalAttack As Boolean = True
    Dim doubleAttack As Boolean = False
    Dim freezeAttack As Boolean = False
    Dim healHeart As Boolean = False
    Dim backupPowerup As String = ""

    'powerup random generation
    Dim poweruprandom As Integer
    Dim randomcount As Integer = 0
    Dim randomselect As Integer = 0


    'score
    Public score As Integer = 0

    Public Function checkpowerup()
        If normalAttack = True Then ' no powerups
            'return like a out of ammo sound effect
        ElseIf doubleAttack = True Then
            tmrPowerup.Enabled = True
            tmrPowerup.Start()
            While tmrPowerup.Interval > 10000
                tmrPowerup.Stop()
                tmrPowerup.Enabled = False
                doubleAttack = False
            End While

        ElseIf freezeAttack = True Then ' or freeze tmr for alien for about 2 seconds
            tmrPowerup.Enabled = True
            tmrPowerup.Start()
            While tmrPowerup.Interval > 10000
                tmrPowerup.Stop()
                tmrPowerup.Enabled = False
            End While
        ElseIf freezeattack = True Then
            tmrPowerup.Enabled = True
            tmrPowerup.Start()
            While tmrPowerup.Interval > 10000
                tmrPowerup.Stop()
                tmrPowerup.Enabled = False
                freezeAttack = False
            End While

        ElseIf healHeart = True Then
            hearts = hearts + 1
            If hearts = 3 Then ' if there is 3 heart left
                Heart3.Image = My.Resources.fullheart
            ElseIf hearts = 2 Then ' if there is 2 heart left
                Heart2.Image = My.Resources.fullheart
            Else ' 1 heart left and the player got hit so game over
                Heart1.Image = My.Resources.fullheart
            End If
        End If
    End Function


    Public Function checkhearts() 'add sound effect for getting hit
        If hearts = 3 Then ' if there is 3 heart left
            Heart3.Image = My.Resources.emptyheart
            hearts = hearts - 1
            Return playerhit = False
        ElseIf hearts = 2 Then ' if there is 2 heart left
            Heart2.Image = My.Resources.emptyheart
            hearts = hearts - 1
            Return playerhit = False
        Else ' 1 heart left and the player got hit so game over
            Heart1.Image = My.Resources.emptyheart
            hearts = hearts - 1
            tmrenemy.Stop()
            tmrMove.Stop()
            tmrShoot.Stop()
            tmrPowerup.Stop()
            tmrrandomiser.Stop()
            gameover.Show()
            Return Gameoverboolean = True
            'run gamer over screen, or display text and lock actions
        End If
    End Function

    Public Function hit()
        playerhit = True
        checkhearts()
    End Function

    Public Function insideBoundary()
        If player.Left < 0 Then
            player.Left = 1 ' keep within form
            Return False
        ElseIf player.Left > 1188 Then ' form width - player size = 1188
            player.Left = 1187
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub createProj(number)
        For i = 0 To number - 1
            Dim projectile As New PictureBox 'create projectile
            projectile.Size = New Size(7, 20)
            projectile.BackColor = Color.White
            projectile.BringToFront()
            Me.Controls.Add(projectile) 'adds picture to form, prevents crashes
            projArray(i) = projectile  'adds projectile to array
            projArray(i).Visible = False
            projOnScreen(i) = False
        Next
    End Sub
    Public Sub createEnemy(number)
        For i = 0 To number - 1
            Dim enemy As New PictureBox
            enemy.Size = New Size(50, 50)
            enemy.BackgroundImageLayout = ImageLayout.Stretch
            enemy.BackgroundImage = My.Resources.alien
            enemy.Top = 100
            enemy.Left = i * 70 + 50
            enemy.BringToFront()
            Me.Controls.Add(enemy)
            enemyArray(i) = enemy
            enemyArray(i).Visible = True
            enemyOnScreen(i) = True
        Next
    End Sub
    Private Sub Form2_KeyPress(ByVal sender As Object, ByVal e As KeyEventArgs) Handles Me.KeyDown
        Dim count As Integer = 1
        Select Case e.KeyCode
            Case Settings.KeyLeft 'move left
                playerLeft = True
            Case Settings.KeyRight 'move right
                playerRight = True
            Case Settings.KeyPowerUp 'powerup
                numofshots = 3
                checkhearts()
            Case Keys.Escape
                tmrenemy.Stop()
                tmrMove.Stop()
                tmrShoot.Stop()
                tmrPowerup.Stop()
                tmrrandomiser.Stop()
                pause.Show()
            Case Settings.KeyShoot 'shooting
                shooting = True
                For i = 0 To numofshots - 1
                    If projOnScreen(i) = True Then
                        count += 1
                    End If
                Next
                If count <= numofshots Then 'caps the total bullets shot to 5, adjustable in top of this code
                    projOnScreen(projNum) = True
                    projArray(projNum).Visible = True
                    projArray(projNum).Top = player.Top
                    projArray(projNum).Left = player.Left + (player.Width / 2) - (projArray(projNum).Width / 2) 'projectile comes out of the middle of player
                    projNum += 1
                    If projNum = numofshots Then
                        projNum = 0
                    End If
                End If
        End Select
        If shooting = True Then
            For i = 0 To numofshots - 1
                If projOnScreen(i) = True Then
                    count += 1
                End If
            Next
            If count <= numofshots Then 'caps the total bullets shot to 5, adjustable in top of this code
                projOnScreen(projNum) = True
                projArray(projNum).Visible = True
                projArray(projNum).Top = player.Top
                projArray(projNum).Left = player.Left + (player.Width / 2) - (projArray(projNum).Width / 2) 'projectile comes out of the middle of player
                projNum += 1
                If projNum = numofshots Then
                    projNum = 0
                End If
            End If
        End If
    End Sub

    'Player's sprite, these set out the location and turns the timers on for shooting.
    Private Sub PictureBox1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Dim pfc As New PrivateFontCollection()
        'Apfc.AddFontFile("C:\Path To\PALETX3.ttf")
        'label1.Font = New Font(pfc.Families(0), 16, FontStyle.Regular)
        Me.Size = New Size(1277, 819)
        Me.CenterToScreen()
        tmrShoot.Enabled = True
        tmrMove.Enabled = True
        tmrEnemy.Enabled = True
        player.Left = Me.Width / 2 - player.Width / 2
        player.Top = Me.Height - 2 * player.Height
        player.Size = New Size(88, 48)
        createProj(numofshots)
        createEnemy(maxEnemyNum)

        Heart1.Location = New Point(1100, 20)
        Heart2.Location = New Point(1137, 20)
        Heart3.Location = New Point(1174, 20)
        Heart1.Size = New Size(40, 40)
        Heart2.Size = New Size(40, 40)
        Heart3.Size = New Size(40, 40)

        LIVESLB.Location = New Point(1010, 17)
        ScoreLB.Location = New Point(100, 17)
        LIVESLB.Font = New Font("Segoe UI", 20.0, FontStyle.Regular)
        ScoreLB.Font = New Font("Segoe UI", 20.0, FontStyle.Regular)


        createProj(numofshots)

    End Sub
    'Every 40 ticks, the bullet shoots (spamming keys, makes it shoot faster)
    Private Sub TimerShoot_Tick(sender As Object, e As EventArgs) Handles tmrShoot.Tick 'lags a decent bit so far
        tmrShoot.Interval = 40
        For i = 0 To numofshots - 1
            If projOnScreen(i) = True Then
                For j = 0 To maxEnemyNum - 1
                    If projArray(i).Bounds.IntersectsWith(enemyArray(j).Bounds) Then
                        LIVESLB.Text = "passs single"
                        'enemyArray = enemyArray.Skip(j).ToArray
                        maxEnemyNum = maxEnemyNum - 1
                        score = score + 20
                        ScoreLB.Text = "Score " + Str(score)
                        enemyOnScreen(j) = False ' add item here to delete enemy image
                        projOnScreen(i) = False
                        's
                        Exit For
                    End If
                Next
                projArray(i).Top -= 15
            End If
            If projArray(i).Top <= -15 Then
                projOnScreen(i) = False
            End If
        Next
    End Sub

    'Movement controls, stops the movement when key is lifted
    Private Sub Form2_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Select Case e.KeyCode
            Case Settings.KeyLeft
                playerLeft = False
            Case Settings.KeyRight
                playerRight = False
            Case Settings.KeyShoot
                shooting = False
        End Select
    End Sub

    'Movement, for each tick, moves 5 units 
    Private Sub tmrMove_Tick(sender As Object, e As EventArgs) Handles tmrMove.Tick
        tmrMove.Interval = 1
        If playerRight = True And insideBoundary() = True Then
            player.Left += 5
        ElseIf playerLeft = True And insideBoundary() = True Then
            player.Left -= 5
        End If
    End Sub

    Private Sub tmrrandomiser_Tick(sender As Object, e As EventArgs) Handles tmrrandomiser.Tick ' randomly making a powerup and selecting a random powerup
        tmrMove.Interval = 10
        Randomize()
        poweruprandom = Int((6 * Rnd()) + 1)
        If poweruprandom = 6 Then
            randomcount += 1
        End If

        If randomcount = 40 Then 'set 200
            randomselect = Int((6 * Rnd()) + 1)
            Select Case randomselect
                Case 1
                    powerupscreen.Image = My.Resources.doublepowerup
                Case 2
                    powerupscreen.Image = My.Resources.freezepowerup
                Case 3
                    powerupscreen.Image = My.Resources.healpowerup
                Case 4
                    powerupscreen.Image = My.Resources.doublepowerup
                Case 5
                    powerupscreen.Image = My.Resources.freezepowerup
                Case 6
                    powerupscreen.Image = My.Resources.healpowerup
            End Select
            LIVESLB.Text = randomselect
            randomcount = 0
        End If
    End Sub

    Private Sub tmrEnemy_Tick(sender As Object, e As EventArgs) Handles tmrEnemy.Tick
        tmrEnemy.Interval = 1000
        For i = 0 To maxEnemyNum - 1
            If enemyMoveRight = True Then
                enemyArray(i).Left += 20
            End If

        Next

    End Sub
End Class